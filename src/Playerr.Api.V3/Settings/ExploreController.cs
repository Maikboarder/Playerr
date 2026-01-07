using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Linq;
using System.Diagnostics.CodeAnalysis;

namespace Playerr.Api.V3.Settings
{
    [ApiController]
    [Route("api/v3/[controller]")]
    [SuppressMessage("Microsoft.Security", "CA3003:ReviewCodeForFilePathInjectionVulnerabilities")]
    [SuppressMessage("Microsoft.Globalization", "CA1307:SpecifyStringComparison")]
    public class ExploreController : ControllerBase
    {
        [HttpGet]
        public IActionResult ListFolders([FromQuery] string? path)
        {
            try
            {
                // Default to root if no path provided
                string targetPath = string.IsNullOrWhiteSpace(path) ? "/" : path;

                if (targetPath.StartsWith("smb://", StringComparison.OrdinalIgnoreCase))
                {
                    return BadRequest(new { 
                        error = "Network protocol 'smb://' detected. This is not a file system path.",
                        desktopSolution = "Mount the network share in your OS first (Finder: Cmd+K, Windows: Map Network Drive), then use the mounted path.",
                        dockerSolution = "Mount the share on your host system, then add it as a volume in docker-compose.yml (e.g., /mnt/myshare:/media)."
                    });
                }

                // For Windows compatibility during development, if path is empty or "/", use a default drive
                if (targetPath == "/" && System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(System.Runtime.InteropServices.OSPlatform.Windows))
                {
                    targetPath = "C:\\";
                }

                if (!Directory.Exists(targetPath))
                {
                    return BadRequest(new { error = "Directory does not exist" });
                }

                var entries = Directory.EnumerateDirectories(targetPath)
                    .Select(d => 
                    {
                        try 
                        {
                            return new 
                            {
                                Name = Path.GetFileName(d),
                                Path = d.Replace("\\", "/"),
                                IsDirectory = true
                            };
                        }
                        catch { return null; }
                    })
                    .Where(d => d != null)
                    .OrderBy(d => d!.Name)
                    .ToList();

                var parentPath = Path.GetDirectoryName(targetPath)?.Replace("\\", "/");
                
                // If we are at the root, ensure parent is null to hide back button
                if (targetPath == "/" || (targetPath.Length == 3 && targetPath.EndsWith(":/", StringComparison.Ordinal)))
                {
                    parentPath = null;
                }

                return Ok(new
                {
                    CurrentPath = targetPath.Replace("\\", "/"),
                    ParentPath = parentPath,
                    Entries = entries
                });
            }
            catch (UnauthorizedAccessException)
            {
                return Forbid();
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpGet("home")]
        public IActionResult GetHome()
        {
            // In Docker, /media or / is a good start. 
            // In Windows (dev), UserProfile or C:\
            string home = "/";
            
            if (System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(System.Runtime.InteropServices.OSPlatform.Windows))
            {
                home = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            }
            else if (Directory.Exists("/media"))
            {
                home = "/media";
            }

            return Ok(new { Path = home.Replace("\\", "/") });
        }
    }
}
