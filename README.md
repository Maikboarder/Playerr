# Playerr
[🇪🇸 Leer en Español](README.es.md)

### **Self-Hosted Game Library Manager & PVR**

[![Go to Website](https://img.shields.io/badge/Website-playerr.app-6366f1?style=for-the-badge)](https://playerr.app)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg?style=for-the-badge)](https://opensource.org/licenses/MIT)
[![Docker Support](https://img.shields.io/badge/Docker-amd64%20%2F%20arm64-2496ed?style=for-the-badge&logo=docker)](https://hub.docker.com/r/maikboarder/playerr)

### Downloads (Latest)
[![Windows](https://img.shields.io/badge/Windows-0078D4?style=for-the-badge&logo=windows&logoColor=white)](https://github.com/Maikboarder/Playerr/releases/latest/download/Playerr-Windows-x64.zip)
[![Playerr.exe](https://img.shields.io/badge/Playerr.exe-Installer-0078D4?style=for-the-badge&logo=windows&logoColor=white)](https://github.com/Maikboarder/Playerr/releases/latest/download/Playerr-Windows-Setup-x64.exe)
[![Playerr.app](https://img.shields.io/badge/Playerr.app-macOS-000000?style=for-the-badge&logo=apple&logoColor=white)](https://github.com/Maikboarder/Playerr/releases/latest/download/Playerr.dmg)
[![macOS ARM64](https://img.shields.io/badge/macOS_Apple_Silicon-000000?style=for-the-badge&logo=apple&logoColor=white)](https://github.com/Maikboarder/Playerr/releases/latest/download/Playerr.dmg)
[![macOS Intel](https://img.shields.io/badge/macOS_Intel-000000?style=for-the-badge&logo=apple&logoColor=white)](https://github.com/Maikboarder/Playerr/releases/latest/download/Playerr-Intel.dmg)
[![Linux](https://img.shields.io/badge/Linux-FCC624?style=for-the-badge&logo=linux&logoColor=black)](https://github.com/Maikboarder/Playerr/releases/latest/download/Playerr-Linux-x64.tar.gz)

Inspired by the workflow of Radarr and Sonarr, Playerr is designed to be the definitive solution for video game enthusiasts who self-host their libraries. It bridges the gap between your local digital assets and the vast world of gaming metadata.

## Main Features

*   **Intelligent Library Scanning:** Recursive and smart recognition engine that identifies video game platforms across your storage, mapping local files to their respective titles.
*   **Rich Metadata Integration:** Native hooks into IGDB and Steam APIs to fetch high-quality artwork, descriptions, ratings, and release dates.
*   **Seamless PVR Workflow:** Support for Prowlarr and Jackett for automated indexer management and advanced searching.
*   **NZB Protocol Support:** Native integration for Usenet downloads via NZB files, automatically handling protocol associations. Compatible with **SABnzbd** and **NZBGet**.
*   **Download Client Connectivity:** Native API integration for managing transfers via industry-standard clients (qBittorrent, Transmission, SABnzbd).
*   **Modern Web GUI:** A vibrant, dark-themed responsive interface designed for both desktop and containerized environments.
*   **Smart Path & File Management:** Automatic folder renaming based on sanitized IGDB titles, preserving original release structure while keeping the library clean and organized.
*   **Automated Deployment Tool:** Efficiently processes local installation packages and identifies primary executables to streamline library organization.
*   **Unified Library View:** Display your entire gaming collection in one place, including native support for syncing and viewing your **Steam Library**.

## Screenshots

| Game View | Game Details |
|:---:|:---:|
| ![Game View](https://raw.githubusercontent.com/Maikboarder/Playerr/master/screenshots/Game%20View.png) | ![Game Details](https://raw.githubusercontent.com/Maikboarder/Playerr/master/screenshots/Details.png) |

| Settings (Indexers) | Library Grid |
|:---:|:---:|
| ![Settings](https://raw.githubusercontent.com/Maikboarder/Playerr/master/screenshots/Indexers%3ATorrents.png) | ![Library Grid](https://raw.githubusercontent.com/Maikboarder/Playerr/master/screenshots/Library%20Games.png) |

<p align="center">
  <img src="https://raw.githubusercontent.com/Maikboarder/Playerr/master/screenshots/Search%20Manager.png" alt="Search Manager" width="600">
  <br>
  <em>Search Manager</em>
</p>

## Supported Platforms

Playerr is architected for maximum reach, offering multi-platform binaries and containerized solutions:

*   **Docker:** Universal support for amd64 and arm64 (Raspberry Pi, CasaOS, Synology, etc.).
*   **Windows:** Native 64-bit performance. [Download .exe (Installer)](https://github.com/Maikboarder/Playerr/releases/latest/download/Playerr-Windows-Setup-x64.exe) or [Download .zip (Portable)](https://github.com/Maikboarder/Playerr/releases/latest/download/Playerr-Windows-x64.zip)
*   **macOS:** Optimized for Apple Silicon ([Download .dmg](https://github.com/Maikboarder/Playerr/releases/latest/download/Playerr.dmg)) and Intel ([Download .dmg](https://github.com/Maikboarder/Playerr/releases/latest/download/Playerr-Intel.dmg)).
*   **Linux:** Generic 64-bit binary distributions. [Download .tar.gz](https://github.com/Maikboarder/Playerr/releases/latest/download/Playerr-Linux-x64.tar.gz)

### Compatibility & Runners
Playerr manages your library, but to execute Windows-native titles on macOS or Linux, we recommend the following compatibility layers:

* **macOS:** [Whisky](https://getwhisky.app/) (Free/Open Source) or [CrossOver](https://www.codeweavers.com/crossover) (Paid/Official Support).
* **Linux / Steam Deck:** Native support via **Proton** (Steam), [Lutris](https://lutris.net/), or [Bottles](https://usebottles.com/).

## Installation & Setup

> **Note:** Requires valid IGDB API keys (free) for metadata fetching.

### Docker (Recommended)
The easiest way to run Playerr is using Docker. It includes everything you need in a single container. Access the UI at `http://your-ip:2727`.

#### Standard Desktop / Server
Create a `docker-compose.yml` file and run `docker-compose up -d`:
```yaml
services:
  playerr:
    image: maikboarder/playerr:latest
    container_name: playerr
    ports:
      - "2727:2727"
    volumes:
      - ./config:/app/config
      - /your/games/path:/media
    restart: unless-stopped
```

#### CasaOS
1. Go to **App Store** -> **Custom Install**.
2. Click on **Import** (top right) and paste this specific code (includes the icon):
   ```yaml
   services:
     playerr:
       image: maikboarder/playerr:latest
       container_name: playerr
       ports:
         - "2727:2727"
       volumes:
         - /DATA/AppData/playerr/config:/app/config
         - /DATA/Media/Games:/media
       restart: unless-stopped
   
   x-casaos:
     architectures:
       - amd64
       - arm64
     main: playerr
     icon: https://raw.githubusercontent.com/Maikboarder/Playerr/master/frontend/src/assets/app_logo.png
     title:
       en_us: Playerr
   ```
3. Click **Install**.

#### Synology / NAS
1. Open **Container Manager** (or Docker).
2. Go to **Project** -> **Create**.
3. Paste the Docker Compose code and configure your local folders.
4. Click **Done**.

### Build from Source (For Developers)

If you want to modify the code or build the image locally instead of pulling it from Docker Hub:

1. Clone the repository:
   ```bash
   git clone https://github.com/maikboarder/playerr.git
   cd playerr
   ```

2. Use the build command:
   ```bash
   docker build -t playerr:local .
   ```

3. Or use a `docker-compose.override.yml` to force a local build:
   ```yaml
   services:
     playerr:
       build: .
       image: playerr:local
       # ... rest of your config
   ```

---

## One-Click Deploy

[![Deploy on RepoCloud](https://d16t0pc4846x52.cloudfront.net/deploylobe.svg)](https://repocloud.io/details/Playerr/)

## Roadmap

### Phase 1: Foundation (v0.1.0 - v0.4.7)
- [x] **Core PVR Functionality:** Automated search and categorization engine.
- [x] **NZB Protocol Support:** Native integration for SABnzbd and NZBGet.
- [x] **Multi-Platform Deployment:** Official builds for Windows, macOS (Apple & Intel), and Linux.
- [x] **Windows Installer:** Professional NSIS installer for a seamless setup experience.
- [x] **Persistent Storage:** SQLite integration to ensure library data and metadata longevity.

### Phase 2: Power User Features (Current Focus)
- [x] **Infrastructure & Storage Optimization:**
  - [x] **Atomic Move (Hardlinks):** Instant file management without data fragmentation.
  - [x] **Unraid Integration:** Community XML template support (`_unraid/playerr.xml`).
  - [x] **Smart API Handling:** Advanced rate-limiting and batching for metadata providers.
- [x] **One-Click Launch Integration:** Direct execution support for **Steam** and initial implementation for **Installed Local Games**.
- [x] **UI/UX Refinement:** Premium iconography (FontAwesome) and consistent Nord-themed design.
- [x] **Advanced Configuration:**
  - [x] **Custom IP/Port Binding:** Flexible network settings.
  - [x] **System Tray Support:** Minimize to Tray for Windows.

### Phase 3: Ecosystem & Future Vision
- [ ] **Bazzite & Linux Gaming:** Specialized compatibility hooks for Lutris, Proton, and Steam Deck.
- [x] **DBI Protocol Support:** Advanced USB file transfer and management for handheld hardware devices.
- [ ] **Official App Stores:** Integration into official Unraid, CasaOS, and Synology app manifests.
- [ ] **Extensibility Engine:** Support for community-driven scripts and metadata plugins.

## Community & Support

I'm building Playerr with the community in mind. Your feedback is the engine that drives our development.

*   **Contribute:** Found a bug? Have a killer feature idea? Open an issue or a PR!
*   **Support:** If Playerr brings value to your setup, consider supporting the project. Your contributions enable more focused development, better stability, and faster implementation of the roadmap.


[![Sponsor on GitHub](https://img.shields.io/static/v1?label=Sponsor&message=%E2%9D%A4&logo=GitHub&color=%23fe8e86&style=for-the-badge)](https://github.com/sponsors/Maikboarder)

## License

Distributed under the MIT License. See `LICENSE` for more information.

## Legal Disclaimer

Playerr is an open-source project for educational and personal library management. It is **not affiliated** with any third-party game platforms or metadata providers. The developers do not condone piracy; users are responsible for complying with their local laws regarding copyright and content usage. See `DISCLAIMER.md` for the full legal notice.

---
*Developed by Maikboarder*
