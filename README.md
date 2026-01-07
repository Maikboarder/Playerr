# Playerr v0.3.0
### **Self-Hosted Game Library Manager & PVR**

[![Go to Website](https://img.shields.io/badge/Website-playerr.app-6366f1?style=for-the-badge)](https://playerr.app)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg?style=for-the-badge)](https://opensource.org/licenses/MIT)
[![Docker Support](https://img.shields.io/badge/Docker-amd64%20%2F%20arm64-2496ed?style=for-the-badge&logo=docker)](https://hub.docker.com/r/maikboarder/playerr)

### Downloads (v0.3.0)
[![Windows](https://img.shields.io/badge/Windows-0078D4?style=for-the-badge&logo=windows&logoColor=white)](https://github.com/Maikboarder/Playerr/releases/download/v0.3.0/Playerr-Windows-x64.zip)
[![Playerr.exe](https://img.shields.io/badge/Playerr.exe-Installer-0078D4?style=for-the-badge&logo=windows&logoColor=white)](https://github.com/Maikboarder/Playerr/releases/download/v0.3.0/Playerr-Windows-Setup-x64.exe)
[![Playerr.app](https://img.shields.io/badge/Playerr.app-macOS-000000?style=for-the-badge&logo=apple&logoColor=white)](https://github.com/Maikboarder/Playerr/releases/download/v0.3.0/Playerr.dmg)
[![macOS ARM64](https://img.shields.io/badge/macOS_Apple_Silicon-000000?style=for-the-badge&logo=apple&logoColor=white)](https://github.com/Maikboarder/Playerr/releases/download/v0.3.0/Playerr.dmg)
[![macOS Intel](https://img.shields.io/badge/macOS_Intel-000000?style=for-the-badge&logo=apple&logoColor=white)](https://github.com/Maikboarder/Playerr/releases/download/v0.3.0/Playerr-Intel.dmg)
[![Linux](https://img.shields.io/badge/Linux-FCC624?style=for-the-badge&logo=linux&logoColor=black)](https://github.com/Maikboarder/Playerr/releases/download/v0.3.0/Playerr-Linux-x64.tar.gz)

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
*   **Windows:** Native 64-bit performance. [Download .exe (Installer)](https://github.com/Maikboarder/Playerr/releases/download/v0.3.0/Playerr-Windows-Setup-x64.exe) or [Download .zip (Portable)](https://github.com/Maikboarder/Playerr/releases/download/v0.3.0/Playerr-Windows-x64.zip)
*   **macOS:** Optimized for Apple Silicon ([Download .dmg](https://github.com/Maikboarder/Playerr/releases/download/v0.3.0/Playerr.dmg)) and Intel ([Download .dmg](https://github.com/Maikboarder/Playerr/releases/download/v0.3.0/Playerr-Intel.dmg)).
*   **Linux:** Generic 64-bit binary distributions. [Download .tar.gz](https://github.com/Maikboarder/Playerr/releases/download/v0.3.0/Playerr-Linux-x64.tar.gz)

## Installation & Setup

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

#### Using Network Shares with Docker

**Important:** Docker containers access files through the host filesystem, not directly via network protocols.

**The Right Way:**
1. **Mount the network share on your host system first**:
   - **Linux:** Mount to a local path (e.g., `/mnt/games`)
     ```bash
     # Create mount point
     sudo mkdir -p /mnt/games
     
     # Mount SMB/CIFS share (example)
     sudo mount -t cifs //192.168.1.100/Games /mnt/games -o username=youruser,password=yourpass
     
     # Or add to /etc/fstab for automatic mounting
     //192.168.1.100/Games /mnt/games cifs credentials=/home/user/.smbcredentials,uid=1000,gid=1000 0 0
     ```
   - **macOS:** Use Finder (Cmd+K) to mount, then reference `/Volumes/ShareName`
   - **Windows:** Map network drive (e.g., Z:\), then use that drive letter

2. **Then bind-mount that path into your Docker container**:
   ```yaml
   services:
     playerr:
       image: maikboarder/playerr:latest
       container_name: playerr
       ports:
         - "2727:2727"
       volumes:
         - ./config:/app/config
         - /mnt/games:/media              # Linux: Your mounted network share
         # - /Volumes/ShareName:/media    # macOS: Mounted share
         # - Z:\:/media                   # Windows: Mapped network drive
       restart: unless-stopped
   ```

**Why can't I use `smb://` directly in Docker?**
- `smb://192.168.1.100/Games` is a **network protocol URL**, not a filesystem path
- Docker volumes require **actual filesystem paths** that the host OS has already mounted
- Desktop apps can sometimes use OS-level network APIs, but Docker is isolated from those
- **Solution:** Always mount network shares on the host first, then pass the mounted path to Docker

**Note about file operations:**
- Playerr uses hardlinks for instant file moves when possible (zero disk space duplication)
- Hardlinks **do not work** across different filesystems or network shares
- When detected, Playerr automatically falls back to standard file copying
- This is normal behavior and ensures compatibility with all storage configurations

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

## Roadmap

### Phase 1: Foundation (v0.1.0 - v0.1.2)
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
- [ ] **One-Click Launch Integration:** Direct execution support for installed local assets with automated path detection.
- [x] **UI/UX Refinement:** Premium iconography (FontAwesome) and consistent Nord-themed design.

### Phase 3: Ecosystem & Future Vision
- [ ] **Bazzite & Linux Gaming:** Specialized compatibility hooks for Lutris, Proton, and Steam Deck.
- [ ] **DBI Protocol Support:** Advanced USB file transfer and management for handheld hardware devices.
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

---

# Playerr v0.3.0
### Gestor de Biblioteca de Videojuegos & PVR (Self-Hosted)

[![Ir a la web](https://img.shields.io/badge/Website-playerr.app-6366f1?style=for-the-badge)](https://playerr.app)
[![Licencia: MIT](https://img.shields.io/badge/License-MIT-yellow.svg?style=for-the-badge)](https://opensource.org/licenses/MIT)
[![Soporte Docker](https://img.shields.io/badge/Docker-amd64%20%2F%20arm64-2496ed?style=for-the-badge&logo=docker)](https://hub.docker.com/r/maikboarder/playerr)

### Descargas (v0.3.0)
[![Windows](https://img.shields.io/badge/Windows-0078D4?style=for-the-badge&logo=windows&logoColor=white)](https://github.com/Maikboarder/Playerr/releases/download/v0.3.0/Playerr-Windows-x64.zip)
[![Playerr.exe](https://img.shields.io/badge/Playerr.exe-Installer-0078D4?style=for-the-badge&logo=windows&logoColor=white)](https://github.com/Maikboarder/Playerr/releases/download/v0.3.0/Playerr-Windows-Setup-x64.exe)
[![Playerr.app](https://img.shields.io/badge/Playerr.app-macOS-000000?style=for-the-badge&logo=apple&logoColor=white)](https://github.com/Maikboarder/Playerr/releases/download/v0.3.0/Playerr.dmg)
[![macOS ARM64](https://img.shields.io/badge/macOS_Apple_Silicon-000000?style=for-the-badge&logo=apple&logoColor=white)](https://github.com/Maikboarder/Playerr/releases/download/v0.3.0/Playerr.dmg)
[![macOS Intel](https://img.shields.io/badge/macOS_Intel-000000?style=for-the-badge&logo=apple&logoColor=white)](https://github.com/Maikboarder/Playerr/releases/download/v0.3.0/Playerr-Intel.dmg)
[![Linux](https://img.shields.io/badge/Linux-FCC624?style=for-the-badge&logo=linux&logoColor=black)](https://github.com/Maikboarder/Playerr/releases/download/v0.3.0/Playerr-Linux-x64.tar.gz)

Inspirado en el flujo de trabajo de Radarr y Sonarr, Playerr está diseñado para ser la solución definitiva para los entusiastas de los videojuegos que gestionan sus bibliotecas en local. Playerr conecta tus archivos digitales con el mundo del metadato gamer.

---

## Características principales

*   **Escaneo inteligente de biblioteca:** Reconocimiento recursivo y automático de plataformas de videojuegos en tu almacenamiento, mapeando archivos locales a sus títulos correspondientes.
*   **Integración de metadatos:** Conexión nativa con IGDB y Steam para obtener imágenes, descripciones, valoraciones y fechas de lanzamiento.
*   **Flujo PVR automatizado:** Soporte para Prowlarr y Jackett para gestión avanzada de indexadores y búsquedas.
*   **Soporte Protocolo NZB:** Integración nativa para descargas Usenet mediante archivos NZB, gestionando automáticamente la asociación de protocolos. Compatible con **SABnzbd** y **NZBGet**.
*   **Conectividad con Clientes de Descarga:** Integración nativa mediante API para gestionar transferencias en los estándares de la industria (qBittorrent, Transmission, SABnzbd).
*   **Interfaz web moderna:** GUI oscura, responsiva y pensada para escritorio y contenedores.
*   **Gestión automática de rutas y archivos:** Renombrado inteligente de carpetas basado en títulos de IGDB, preservando la estructura original mientras se mantiene la biblioteca limpia y organizada.
*   **Herramienta de Despliegue Automatizado:** Procesa eficientemente paquetes de instalación locales e identifica los ejecutables primarios para optimizar la organización de la biblioteca.
*   **Vista unificada:** Muestra toda tu colección en un solo lugar, incluyendo sincronización y visualización de tu biblioteca de Steam.

## Plataformas soportadas

Playerr está diseñado para máxima compatibilidad, ofreciendo binarios multiplataforma y soluciones en contenedor:

*   **Docker:** Soporte universal para amd64 y arm64 (Raspberry Pi, CasaOS, Synology, etc.).
*   **Windows:** Rendimiento nativo 64-bit. [Descargar .exe (Instalador)](https://github.com/Maikboarder/Playerr/releases/download/v0.3.0/Playerr-Windows-Setup-x64.exe) o [Descargar .zip (Portable)](https://github.com/Maikboarder/Playerr/releases/download/v0.3.0/Playerr-Windows-x64.zip)
*   **macOS:** Optimizado para Apple Silicon ([Descargar .dmg](https://github.com/Maikboarder/Playerr/releases/download/v0.3.0/Playerr.dmg)) y sistemas Intel ([Descargar .dmg](https://github.com/Maikboarder/Playerr/releases/download/v0.3.0/Playerr-Intel.dmg)).
*   **Linux:** Distribuciones genéricas 64-bit. [Descargar .tar.gz](https://github.com/Maikboarder/Playerr/releases/download/v0.3.0/Playerr-Linux-x64.tar.gz)

## Instalación y configuración

### Docker (Recomendado)
La forma más sencilla de ejecutar Playerr es con Docker. Incluye todo lo necesario en un solo contenedor. Accede a la interfaz en `http://tu-ip:2727`.

#### Escritorio / Servidor estándar
Crea un archivo `docker-compose.yml` y ejecuta `docker-compose up -d`:
```yaml
services:
  playerr:
    image: maikboarder/playerr:latest
    container_name: playerr
    ports:
      - "2727:2727"
    volumes:
      - ./config:/app/config
      - /ruta/a/tus/juegos:/media
    restart: unless-stopped
```

#### CasaOS
1. Ve a **App Store** -> **Instalación personalizada**.
2. Haz clic en **Importar** (arriba a la derecha) y pega este código (incluye el icono):
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
       es_es: Playerr
   ```
3. Haz clic en **Instalar**.

#### Synology / NAS
1. Abre **Container Manager** (o Docker).
2. Ve a **Proyecto** -> **Crear**.
3. Pega el código y configura tus carpetas locales.
4. Haz clic en **Listo**.

#### Usando Carpetas Compartidas en Red con Docker

**Importante:** Los contenedores Docker acceden a los archivos a través del sistema de archivos del host, no directamente mediante protocolos de red.

**La Forma Correcta:**
1. **Monta la carpeta compartida en tu sistema host primero**:
   - **Linux:** Montar en una ruta local (ej., `/mnt/juegos`)
     ```bash
     # Crear punto de montaje
     sudo mkdir -p /mnt/juegos
     
     # Montar carpeta compartida SMB/CIFS (ejemplo)
     sudo mount -t cifs //192.168.1.100/Juegos /mnt/juegos -o username=tuusuario,password=tupass
     
     # O agregar a /etc/fstab para montaje automático
     //192.168.1.100/Juegos /mnt/juegos cifs credentials=/home/user/.smbcredentials,uid=1000,gid=1000 0 0
     ```
   - **macOS:** Usa Finder (Cmd+K) para montar, luego usa `/Volumes/NombreRecurso`
   - **Windows:** Mapea la unidad de red (ej., Z:\), luego usa esa letra de unidad

2. **Luego vincula esa ruta en tu contenedor Docker**:
   ```yaml
   services:
     playerr:
       image: maikboarder/playerr:latest
       container_name: playerr
       ports:
         - "2727:2727"
       volumes:
         - ./config:/app/config
         - /mnt/juegos:/media              # Linux: Tu carpeta de red montada
         # - /Volumes/NombreRecurso:/media # macOS: Recurso montado
         # - Z:\:/media                    # Windows: Unidad de red mapeada
       restart: unless-stopped
   ```

**¿Por qué no puedo usar `smb://` directamente en Docker?**
- `smb://192.168.1.100/Juegos` es una **URL de protocolo de red**, no una ruta del sistema de archivos
- Los volúmenes de Docker requieren **rutas reales del sistema de archivos** que el sistema operativo host ya haya montado
- Las aplicaciones de escritorio a veces pueden usar APIs de red del SO, pero Docker está aislado de esas
- **Solución:** Siempre monta las carpetas de red en el host primero, luego pasa la ruta montada a Docker

**Nota sobre operaciones de archivos:**
- Playerr usa hardlinks para mover archivos instantáneamente cuando es posible (sin duplicar espacio en disco)
- Los hardlinks **no funcionan** entre diferentes sistemas de archivos o carpetas de red
- Cuando se detecta esto, Playerr automáticamente usa copia de archivos estándar
- Este es el comportamiento normal y asegura compatibilidad con todas las configuraciones de almacenamiento

### Compilar desde el código (Desarrolladores)

Si quieres modificar el código o construir la imagen localmente:

1. Clona el repositorio:
   ```bash
   git clone https://github.com/maikboarder/playerr.git
   cd playerr
   ```

2. Usa el comando de compilación:
   ```bash
   docker build -t playerr:local .
   ```

3. O usa un `docker-compose.override.yml` para forzar compilación local:
   ```yaml
   services:
     playerr:
       build: .
       image: playerr:local
       # ... resto de la config
   ```

---

## 🗺️ Roadmap

### Fase 1: Cimientos (v0.1.0 - v0.1.2)
- [x] **Funcionalidad PVR Core:** Motor de búsqueda y categorización automática.
- [x] **Soporte de Protocolo NZB:** Integración nativa con SABnzbd y NZBGet.
- [x] **Despliegue Multiplataforma:** Binarios oficiales para Windows, macOS (Apple e Intel) y Linux.
- [x] **Instalador de Windows:** Instalador profesional NSIS para una experiencia de configuración fluida.
- [x] **Persistencia de Datos:** Integración con SQLite para asegurar la longevidad de tu biblioteca y metadatos.

### Fase 2: Funciones Avanzadas (Enfoque Actual)
- [x] **Optimización de Infraestructura:**
  - [x] **Hardlinks (Atomic Move):** Gestión instantánea de archivos sin fragmentación de datos.
  - [x] **Integración con Unraid:** Soporte mediante plantilla XML comunitaria (`_unraid/playerr.xml`).
  - [x] **Gestión Inteligente de API:** Control de límites y procesamiento por lotes para proveedores de metadatos.
- [ ] **Integración "One-Click Launch":** Soporte de ejecución directa para juegos instalados con detección automática de rutas.
- [x] **Refinamiento de UI/UX:** Iconografía premium (FontAwesome) y diseño consistente basado en el tema Nord.

### Fase 3: Ecosistema y Visión de Futuro
- [ ] **Bazzite y Gaming en Linux:** Hooks de compatibilidad especializados para Lutris, Proton y Steam Deck.
- [ ] **Protocolo DBI:** Transferencia avanzada de archivos por USB para la gestión de dispositivos de hardware portátiles.
- [ ] **Tiendas Oficiales:** Integración en los catálogos oficiales de aplicaciones de Unraid, CasaOS y Synology.
- [ ] **Motor de Extensibilidad:** Soporte para scripts de la comunidad y plugins de metadatos.

## Comunidad y soporte

Estoy construyendo Playerr con la comunidad en mente. Tu feedback es el motor que impulsa nuestro desarrollo.

*   **Contribuye:** ¿Encontraste un bug? ¿Tienes una idea brillante? ¡Abre un issue o un PR!
*   **Soporte:** Si Playerr aporta valor a tu setup, considera apoyar el proyecto. Tus contribuciones permiten un desarrollo más enfocado, mejor estabilidad y una implementación más rápida del roadmap.

[![Sponsor on GitHub](https://img.shields.io/static/v1?label=Sponsor&message=%E2%9D%A4&logo=GitHub&color=%23fe8e86&style=for-the-badge)](https://github.com/sponsors/Maikboarder)

## Licencia

Distribuido bajo la Licencia MIT. Consulta `LICENSE` para más información.

## Aviso Legal

Playerr es un proyecto de código abierto para la gestión educativa y personal de bibliotecas. **No está afiliado** con ninguna plataforma de juegos o proveedor de metadatos de terceros. Los desarrolladores no aprueban la piratería; los usuarios son responsables de cumplir con las leyes locales de derechos de autor. Consulta `DISCLAIMER.md` para el aviso legal completo.

---
*Desarrollado por Maikboarder*

