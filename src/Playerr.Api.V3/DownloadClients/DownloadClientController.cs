using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Playerr.Core.Download;
using Playerr.Core.Configuration;
using System.Diagnostics.CodeAnalysis;

namespace Playerr.Api.V3.DownloadClients
{
    [ApiController]
    [Route("api/v3/downloadclient")]
    [SuppressMessage("Microsoft.Performance", "CA1860:AvoidUsingAnyWhenUseCount")]
    [SuppressMessage("Microsoft.Maintainability", "CA1508:AvoidDeadConditionalCode")]
    [SuppressMessage("Microsoft.Design", "CA1056:UriPropertiesShouldNotBeStrings")]
    public class DownloadClientController : ControllerBase
    {
        private readonly List<DownloadClient> _clients;
        private readonly ConfigurationService _configService;
        private readonly ImportStatusService _importStatus;

        public DownloadClientController(ConfigurationService configService, ImportStatusService importStatus)
        {
            _configService = configService;
            _importStatus = importStatus;
            _clients = _configService.LoadDownloadClients();
        }

        [HttpGet]
        public ActionResult<List<DownloadClient>> GetAll()
        {
            return Ok(_clients);
        }

        [HttpGet("{id}")]
        public ActionResult<DownloadClient> GetById(int id)
        {
            var client = _clients.FirstOrDefault(c => c.Id == id);
            if (client == null)
            {
                return NotFound();
            }
            return Ok(client);
        }

        [HttpPost]
        public ActionResult<DownloadClient> Create([FromBody] DownloadClient client)
        {
            client.Id = _clients.Any() ? _clients.Max(c => c.Id) + 1 : 1;
            _clients.Add(client);
            _configService.SaveDownloadClients(_clients);
            return CreatedAtAction(nameof(GetById), new { id = client.Id }, client);
        }

        [HttpPut("{id}")]
        public ActionResult<DownloadClient> Update(int id, [FromBody] DownloadClient client)
        {
            var existingClient = _clients.FirstOrDefault(c => c.Id == id);
            if (existingClient == null)
            {
                return NotFound();
            }

            existingClient.Name = client.Name;
            existingClient.Implementation = client.Implementation;
            existingClient.Host = client.Host;
            existingClient.Port = client.Port;
            existingClient.Username = client.Username;
            existingClient.Password = client.Password;
            existingClient.Category = client.Category;
            existingClient.UrlBase = client.UrlBase;
            existingClient.ApiKey = client.ApiKey;
            existingClient.Enable = client.Enable;
            existingClient.Priority = client.Priority;
            existingClient.RemotePathMapping = client.RemotePathMapping;
            existingClient.LocalPathMapping = client.LocalPathMapping;

            _configService.SaveDownloadClients(_clients);

            return Ok(existingClient);
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            var client = _clients.FirstOrDefault(c => c.Id == id);
            if (client == null)
            {
                return NotFound();
            }

            _clients.Remove(client);
            _configService.SaveDownloadClients(_clients);
            return NoContent();
        }

        [HttpGet("queue")]
        public async Task<ActionResult<List<DownloadStatus>>> GetQueue()
        {
            var allDownloads = new List<DownloadStatus>();

            foreach (var config in _clients.Where(c => c.Enable))
            {
                IDownloadClient? client = null;
                if (config.Implementation.Equals("qBittorrent", StringComparison.OrdinalIgnoreCase))
                {
                    client = new QBittorrentClient(config.Host, config.Port, config.Username ?? "", config.Password ?? "", config.UrlBase);
                }
                else if (config.Implementation.Equals("Transmission", StringComparison.OrdinalIgnoreCase))
                {
                    client = new TransmissionClient(config.Host, config.Port, config.Username ?? "", config.Password ?? "");
                }
                else if (config.Implementation.Equals("SABnzbd", StringComparison.OrdinalIgnoreCase))
                {
                    client = new SabnzbdClient(config.Host, config.Port, config.ApiKey ?? "", config.UrlBase);
                }
                else if (config.Implementation.Equals("NZBGet", StringComparison.OrdinalIgnoreCase))
                {
                    client = new NzbgetClient(config.Host, config.Port, config.Username ?? "", config.Password ?? "", config.UrlBase);
                }

                if (client != null)
                {
                    try
                    {
                        var downloads = await client.GetDownloadsAsync();
                        foreach (var d in downloads) 
                        {
                            d.ClientId = config.Id;
                            if (_importStatus.IsImporting(d.Id))
                            {
                                d.State = DownloadState.Importing;
                            }
                        }
                        allDownloads.AddRange(downloads);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error fetching downloads for client {config.Name}: {ex.Message}");
                    }
                }
            }

            return Ok(allDownloads);
        }

        [HttpDelete("queue/{clientId}/{downloadId}")]
        public async Task<ActionResult> DeleteDownload(int clientId, string downloadId)
        {
            var result = await HandleDownloadAction(clientId, downloadId, (client, id) => client.RemoveDownloadAsync(id));
            if (result) return Ok();
            return BadRequest("Failed to delete download from client.");
        }

        [HttpPost("queue/{clientId}/{downloadId}/pause")]
        public async Task<ActionResult> PauseDownload(int clientId, string downloadId)
        {
            var result = await HandleDownloadAction(clientId, downloadId, (client, id) => client.PauseDownloadAsync(id));
            if (result) return Ok();
            return BadRequest("Failed to pause download.");
        }

        [HttpPost("queue/{clientId}/{downloadId}/resume")]
        public async Task<ActionResult> ResumeDownload(int clientId, string downloadId)
        {
            var result = await HandleDownloadAction(clientId, downloadId, (client, id) => client.ResumeDownloadAsync(id));
            if (result) return Ok();
            return BadRequest("Failed to resume download.");
        }

        private async Task<bool> HandleDownloadAction(int clientId, string downloadId, Func<IDownloadClient, string, Task<bool>> action)
        {
            var config = _clients.FirstOrDefault(c => c.Id == clientId);
            if (config == null) return false;

            IDownloadClient? client = null;
            if (config.Implementation.Equals("qBittorrent", StringComparison.OrdinalIgnoreCase))
                client = new QBittorrentClient(config.Host, config.Port, config.Username ?? "", config.Password ?? "", config.UrlBase);
            else if (config.Implementation.Equals("Transmission", StringComparison.OrdinalIgnoreCase))
                client = new TransmissionClient(config.Host, config.Port, config.Username ?? "", config.Password ?? "");
            else if (config.Implementation.Equals("SABnzbd", StringComparison.OrdinalIgnoreCase))
                client = new SabnzbdClient(config.Host, config.Port, config.ApiKey ?? "", config.UrlBase);
            else if (config.Implementation.Equals("NZBGet", StringComparison.OrdinalIgnoreCase))
                client = new NzbgetClient(config.Host, config.Port, config.Username ?? "", config.Password ?? "", config.UrlBase);

            if (client == null) return false;

            try 
            {
                var decodedId = Uri.UnescapeDataString(downloadId);
                return await action(client, decodedId);
            }
            catch { return false; }
        }

        [HttpPost("test")]
        public async Task<ActionResult> TestConnection([FromBody] TestDownloadClientRequest request)
        {
            try
            {
                bool isConnected = false;
                string version = string.Empty;

                Console.WriteLine($"[DownloadClient] Testing {request.Implementation} at {request.Host}:{request.Port}");

                if (request.Implementation.Equals("qBittorrent", StringComparison.OrdinalIgnoreCase))
                {
                    var qbClient = new QBittorrentClient(
                        request.Host,
                        request.Port,
                        request.Username ?? string.Empty,
                        request.Password ?? string.Empty,
                        request.UrlBase
                    );

                    isConnected = await qbClient.TestConnectionAsync();
                    if (isConnected)
                    {
                        version = await qbClient.GetVersionAsync();
                    }
                }
                else if (request.Implementation.Equals("Transmission", StringComparison.OrdinalIgnoreCase))
                {
                    var transmissionClient = new TransmissionClient(
                        request.Host,
                        request.Port,
                        request.Username ?? string.Empty,
                        request.Password ?? string.Empty
                    );

                    isConnected = await transmissionClient.TestConnectionAsync();
                    if (isConnected)
                    {
                        version = await transmissionClient.GetVersionAsync();
                    }
                }
                else if (request.Implementation.Equals("SABnzbd", StringComparison.OrdinalIgnoreCase))
                {
                    var sabClient = new SabnzbdClient(
                        request.Host,
                        request.Port,
                        request.ApiKey ?? string.Empty,
                        request.UrlBase
                    );

                    isConnected = await sabClient.TestConnectionAsync();
                    if (isConnected)
                    {
                        version = await sabClient.GetVersionAsync();
                    }
                }
                else if (request.Implementation.Equals("NZBGet", StringComparison.OrdinalIgnoreCase))
                {
                    var nzbClient = new NzbgetClient(
                        request.Host,
                        request.Port,
                        request.Username ?? string.Empty,
                        request.Password ?? string.Empty,
                        request.UrlBase
                    );

                    isConnected = await nzbClient.TestConnectionAsync();
                    if (isConnected)
                    {
                        version = await nzbClient.GetVersionAsync();
                    }
                }
                else
                {
                    return BadRequest(new { message = $"Unsupported download client: {request.Implementation}" });
                }

                return Ok(new
                {
                    connected = isConnected,
                    version = version,
                    message = isConnected ? "Connection successful" : "Connection failed"
                });
            }
            catch (Exception ex)
            {
                return Ok(new
                {
                    connected = false,
                    message = $"Connection failed: {ex.Message}"
                });
            }
        }

        [HttpPost("add")]
        public async Task<ActionResult> AddTorrent([FromBody] AddTorrentRequest request)
        {
            try
            {
                Console.WriteLine($"[DownloadClient] Attempting to add torrent: {request.Url}");
                
                // Sort by Priority (lower is better, assuming 1 is highest priority)
                // If priorities are equal, use ID (assuming newer clients might be preferred or just stable sort)
                DownloadClient? client = null;
                
                // Smart Selection based on Protocol (Passed from Frontend) or URL extension
                bool isNzb = false;
                
                if (!string.IsNullOrEmpty(request.Protocol))
                {
                    isNzb = request.Protocol.Equals("nzb", StringComparison.OrdinalIgnoreCase);
                }
                else
                {
                    // Fallback to URL check
                    isNzb = request.Url.EndsWith(".nzb", StringComparison.OrdinalIgnoreCase);
                }
                
                Console.WriteLine($"[DownloadClient] Request Protocol: '{request.Protocol}', IsNZB: {isNzb}");
                
                if (isNzb)
                {
                    // Prioritize Usenet clients
                     client = _clients
                        .Where(c => c.Enable && (c.Implementation.Equals("SABnzbd", StringComparison.OrdinalIgnoreCase) || c.Implementation.Equals("NZBGet", StringComparison.OrdinalIgnoreCase)))
                        .OrderBy(c => c.Priority).ThenBy(c => c.Id)
                        .FirstOrDefault();
                }
                else
                {
                    // Prioritize Torrent clients (default)
                     client = _clients
                        .Where(c => c.Enable && !c.Implementation.Equals("SABnzbd", StringComparison.OrdinalIgnoreCase) && !c.Implementation.Equals("NZBGet", StringComparison.OrdinalIgnoreCase))
                        .OrderBy(c => c.Priority).ThenBy(c => c.Id)
                        .FirstOrDefault();
                }

                if (client == null)
                {
                    Console.WriteLine($"[DownloadClient] No enabled download client found for {(isNzb ? "NZB" : "Torrent")}");
                    return BadRequest(new { message = $"No enabled {(isNzb ? "Usenet" : "Torrent")} download client found." });
                }
                
                if (client.Implementation.Equals("qBittorrent", StringComparison.OrdinalIgnoreCase))
                {
                    var qbClient = new QBittorrentClient(
                        client.Host,
                        client.Port,
                        client.Username ?? string.Empty,
                        client.Password ?? string.Empty,
                        client.UrlBase
                    );

                    bool success = await qbClient.AddTorrentAsync(request.Url, client.Category ?? string.Empty);
                    if (success)
                    {
                        Console.WriteLine("[DownloadClient] Successfully added torrent to qBittorrent");
                        return Ok(new { message = "Torrent added successfully to qBittorrent" });
                    }
                    else
                    {
                        Console.WriteLine("[DownloadClient] Failed to add torrent to qBittorrent. It might be an NZB. Attempting failover...");
                        
                        // Failover: Try adding to Usenet client
                        var usenetClient = _clients
                            .Where(c => c.Enable && (c.Implementation.Equals("SABnzbd", StringComparison.OrdinalIgnoreCase) || c.Implementation.Equals("NZBGet", StringComparison.OrdinalIgnoreCase)))
                            .OrderBy(c => c.Priority).ThenBy(c => c.Id)
                            .FirstOrDefault();
                            
                        if (usenetClient != null)
                        {
                            Console.WriteLine($"[DownloadClient] Failover: Found Usenet client {usenetClient.Implementation}. Trying...");
                            if (usenetClient.Implementation.Equals("SABnzbd", StringComparison.OrdinalIgnoreCase))
                            {
                                var sabClient = new SabnzbdClient(usenetClient.Host, usenetClient.Port, usenetClient.ApiKey ?? string.Empty, usenetClient.UrlBase);
                                if (await sabClient.AddNzbAsync(request.Url, usenetClient.Category ?? string.Empty))
                                    return Ok(new { message = "Added to SABnzbd (Failover from Torrent)" });
                            }
                            else if (usenetClient.Implementation.Equals("NZBGet", StringComparison.OrdinalIgnoreCase))
                            {
                                var nzbClient = new NzbgetClient(usenetClient.Host, usenetClient.Port, usenetClient.Username ?? string.Empty, usenetClient.Password ?? string.Empty, usenetClient.UrlBase);
                                if (await nzbClient.AddNzbAsync(request.Url, usenetClient.Category ?? string.Empty))
                                    return Ok(new { message = "Added to NZBGet (Failover from Torrent)" });
                            }
                        }
                        
                        return StatusCode(500, new { message = "Failed to add torrent to qBittorrent and Failover failed." });
                    }
                }
                else if (client.Implementation.Equals("Transmission", StringComparison.OrdinalIgnoreCase))
                {
                    var transmissionClient = new TransmissionClient(
                        client.Host,
                        client.Port,
                        client.Username ?? string.Empty,
                        client.Password ?? string.Empty
                    );

                    bool success = await transmissionClient.AddTorrentAsync(request.Url, client.Category ?? string.Empty);
                    if (success)
                    {
                        Console.WriteLine("[DownloadClient] Successfully added torrent to Transmission");
                        return Ok(new { message = "Torrent added successfully to Transmission" });
                    }
                    else
                    {
                        Console.WriteLine("[DownloadClient] Failed to add torrent to Transmission");
                        return StatusCode(500, new { message = "Failed to add torrent to Transmission" });
                    }
                }
                else if (client.Implementation.Equals("SABnzbd", StringComparison.OrdinalIgnoreCase))
                {
                    var sabClient = new SabnzbdClient(
                        client.Host,
                        client.Port,
                        client.ApiKey ?? string.Empty,
                        client.UrlBase
                    );
                    
                    bool success = await sabClient.AddNzbAsync(request.Url, client.Category ?? string.Empty);
                    if (success)
                    {
                        Console.WriteLine("[DownloadClient] Successfully added NZB to SABnzbd");
                        return Ok(new { message = "NZB added successfully to SABnzbd" });
                    }
                    else
                    {
                         return StatusCode(500, new { message = "Failed to add NZB to SABnzbd" });
                    }
                }
                else if (client.Implementation.Equals("NZBGet", StringComparison.OrdinalIgnoreCase))
                {
                    var nzbClient = new NzbgetClient(
                        client.Host,
                        client.Port,
                        client.Username ?? string.Empty,
                        client.Password ?? string.Empty,
                        client.UrlBase
                    );
                    
                    bool success = await nzbClient.AddNzbAsync(request.Url, client.Category ?? string.Empty);
                    if (success)
                    {
                        Console.WriteLine("[DownloadClient] Successfully added NZB to NZBGet");
                        return Ok(new { message = "NZB added successfully to NZBGet" });
                    }
                    else
                    {
                         return StatusCode(500, new { message = "Failed to add NZB to NZBGet" });
                    }
                }
                
                return BadRequest(new { message = $"Unsupported download client: {client.Implementation}" });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[DownloadClient] Error adding torrent: {ex.Message}");
                return StatusCode(500, new { message = $"Error adding torrent: {ex.Message}" });
            }
        }
    }

    [SuppressMessage("Microsoft.Design", "CA1056:UriPropertiesShouldNotBeStrings")]
    public class AddTorrentRequest
    {
        public string Url { get; set; } = string.Empty;
        public string? Protocol { get; set; } // "torrent", "nzb"
    }

    [SuppressMessage("Microsoft.Design", "CA1056:UriPropertiesShouldNotBeStrings")]
    public class TestDownloadClientRequest
    {
        public string Implementation { get; set; } = string.Empty;
        public string Host { get; set; } = string.Empty;
        public int Port { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
        public string? UrlBase { get; set; }
        public string? ApiKey { get; set; }
    }
}
