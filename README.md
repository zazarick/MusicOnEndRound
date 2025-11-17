<div align="center">

# 🎵 MusicOnEndRound

### *Bring your server to life with epic end-round music*

[![EXILED](https://img.shields.io/badge/EXILED-9.10.2+-blue?style=for-the-badge&logo=github)](https://github.com/Exiled-Team/EXILED)
[![License](https://img.shields.io/badge/License-MIT-green?style=for-the-badge)](LICENSE)
[![SCP:SL](https://img.shields.io/badge/SCP%3ASL-Compatible-red?style=for-the-badge)](https://scpslgame.com/)

**[English](#english) | [Русский](#russian)**

---

</div>

<a name="english"></a>

## 📖 About

**MusicOnEndRound** is a powerful EXILED plugin for SCP: Secret Laboratory that plays custom music at the end of each round. With advanced features like weighted random selection, per-track settings, and automatic track discovery, you can create the perfect atmosphere for your server.

## ✨ Features

- 🌍 **Global Audio** - Music plays for all players simultaneously
- 🎲 **Weighted Random Selection** - Control how often each track plays
- 🎚️ **Per-Track Settings** - Individual volume, chance, and enable/disable for each song
- 🔄 **Auto-Discovery** - Automatically detects and adds new tracks to config
- 🎮 **Admin Commands** - Manual control over music playback
- ⚙️ **Highly Configurable** - Fine-tune every aspect of the plugin

## 📋 Requirements

- **EXILED** 9.10.2 or higher
- **[AudioPlayerApi](https://github.com/Killers0992/AudioPlayerApi)** by Killers0992

### 🎵 Audio File Requirements

> ⚠️ **IMPORTANT:** Audio files must meet these specifications:

| Parameter | Value |
|-----------|-------|
| **Format** | `.ogg` |
| **Channels** | `1 (MONO)` |
| **Sample Rate** | `48000 Hz (48 kHz)` |

Files with different parameters may not play correctly or at all.

## 🚀 Installation

1. Download and install **AudioPlayerApi** to `EXILED/Plugins`
2. Download **MusicOnEndRound.dll** and place it in `EXILED/Plugins`
3. Start your server to generate the default config
4. Add your `.ogg` music files to `EXILED/Configs/MusicOnEndRound`
5. Restart the plugin or server
6. Configure track settings in the config file

## ⚙️ Configuration

```yaml
music_on_end_round:
  is_enabled: true
  debug: false
  music_folder_path: '%exiled%/Configs/MusicOnEndRound'
  delay_before_play: 2
  loop_music: false
  tracks:
    epic_victory:
      enabled: true
      chance: 100
      volume: 60
    dramatic_ending:
      enabled: true
      chance: 75
      volume: 55
    rare_easter_egg:
      enabled: true
      chance: 10
      volume: 70
```

### 🎛️ Main Settings

| Setting | Description |
|---------|-------------|
| `is_enabled` | Enable/disable the plugin |
| `debug` | Show debug messages in console |
| `music_folder_path` | Path to music folder |
| `delay_before_play` | Delay before music starts (seconds) |
| `loop_music` | Loop the music playback |
| `tracks` | Dictionary of track-specific settings |

### 🎼 Track Settings

Each track (key = filename without extension) has:

| Setting | Description |
|---------|-------------|
| `enabled` | Enable/disable this track |
| `chance` | Playback chance weight (0-100) |
| `volume` | Track-specific volume (0-100) |

#### 📊 How Chance System Works

The plugin uses **weighted random selection**. Higher chance values = more frequent playback.

**Example:** 3 tracks with chances `100`, `50`, `25`:
- Track 1: ~57% playback rate
- Track 2: ~29% playback rate  
- Track 3: ~14% playback rate

## 🎮 Commands

### `playendmusic` (alias: `pem`)
Manually play music from the plugin folder.

**Usage:**
- `playendmusic` - Play random track
- `playendmusic trackname` - Play specific track

**Permission:** `musiconendround.play`

### `stopendmusic` (alias: `sem`)
Stop currently playing music.

**Usage:** `stopendmusic`

**Permission:** `musiconendround.stop`

## 🛠️ For Developers

### Project Structure
```
MusicOnEndRound/
├── Commands/
│   ├── PlayMusicCommand.cs
│   └── StopMusicCommand.cs
├── EventHandlers/
│   └── ServerHandlers.cs
├── Models/
│   └── TrackSettings.cs
├── Config.cs
└── Plugin.cs
```

## 👤 Author

**Zazar**

---

<a name="russian"></a>

## 📖 О плагине

**MusicOnEndRound** - мощный плагин для EXILED в SCP: Secret Laboratory, который воспроизводит музыку в конце каждого раунда. С продвинутыми функциями, такими как взвешенный случайный выбор, персональные настройки для каждого трека и автоматическое обнаружение треков, вы можете создать идеальную атмосферу для вашего сервера.

## ✨ Возможности

- 🌍 **Глобальное аудио** - Музыка играет для всех игроков одновременно
- 🎲 **Взвешенный случайный выбор** - Контролируйте частоту воспроизведения каждого трека
- 🎚️ **Настройки для каждого трека** - Индивидуальная громкость, шанс и включение/выключение
- 🔄 **Автообнаружение** - Автоматически находит и добавляет новые треки в конфиг
- 🎮 **Команды администратора** - Ручное управление воспроизведением
- ⚙️ **Гибкая настройка** - Настройте каждый аспект плагина

## 📋 Требования

- **EXILED** 9.10.2 или выше
- **[AudioPlayerApi](https://github.com/Killers0992/AudioPlayerApi)** от Killers0992

### 🎵 Требования к аудиофайлам

> ⚠️ **ВАЖНО:** Аудиофайлы должны соответствовать следующим требованиям:

| Параметр | Значение |
|----------|----------|
| **Формат** | `.ogg` |
| **Каналы** | `1 (MONO)` |
| **Частота** | `48000 Hz (48 kHz)` |

Файлы с другими параметрами могут не воспроизводиться или работать некорректно.

## 🚀 Установка

1. Скачайте и установите **AudioPlayerApi** в `EXILED/Plugins`
2. Скачайте **MusicOnEndRound.dll** и поместите в `EXILED/Plugins`
3. Запустите сервер для генерации конфига
4. Добавьте ваши `.ogg` файлы в `EXILED/Configs/MusicOnEndRound`
5. Перезапустите плагин или сервер
6. Настройте треки в конфигурационном файле

## ⚙️ Конфигурация

```yaml
music_on_end_round:
  is_enabled: true
  debug: false
  music_folder_path: '%exiled%/Configs/MusicOnEndRound'
  delay_before_play: 2
  loop_music: false
  tracks:
    epic_victory:
      enabled: true
      chance: 100
      volume: 60
    dramatic_ending:
      enabled: true
      chance: 75
      volume: 55
    rare_easter_egg:
      enabled: true
      chance: 10
      volume: 70
```

### 🎛️ Основные настройки

| Настройка | Описание |
|-----------|----------|
| `is_enabled` | Включить/выключить плагин |
| `debug` | Показывать отладочные сообщения |
| `music_folder_path` | Путь к папке с музыкой |
| `delay_before_play` | Задержка перед началом (секунды) |
| `loop_music` | Зациклить воспроизведение |
| `tracks` | Словарь настроек для каждого трека |

### 🎼 Настройки треков

Каждый трек (ключ = имя файла без расширения) имеет:

| Настройка | Описание |
|-----------|----------|
| `enabled` | Включить/выключить трек |
| `chance` | Вес шанса воспроизведения (0-100) |
| `volume` | Громкость трека (0-100) |

#### 📊 Как работает система шансов

Плагин использует **взвешенный случайный выбор**. Чем выше значение шанса = чаще воспроизведение.

**Пример:** 3 трека с шансами `100`, `50`, `25`:
- Трек 1: ~57% частота воспроизведения
- Трек 2: ~29% частота воспроизведения
- Трек 3: ~14% частота воспроизведения

## 🎮 Команды

### `playendmusic` (алиас: `pem`)
Вручную воспроизвести музыку из папки плагина.

**Использование:**
- `playendmusic` - Воспроизвести случайный трек
- `playendmusic название` - Воспроизвести конкретный трек

**Права:** `musiconendround.play`

### `stopendmusic` (алиас: `sem`)
Остановить воспроизведение музыки.

**Использование:** `stopendmusic`

**Права:** `musiconendround.stop`

## 🛠️ Для разработчиков

### Структура проекта
```
MusicOnEndRound/
├── Commands/
│   ├── PlayMusicCommand.cs
│   └── StopMusicCommand.cs
├── EventHandlers/
│   └── ServerHandlers.cs
├── Models/
│   └── TrackSettings.cs
├── Config.cs
└── Plugin.cs
```

## 👤 Автор

**Zazar**

---

<div align="center">

### 💝 Support

If you like this plugin, consider giving it a ⭐ on GitHub!

**Made with ❤️ for the SCP:SL community**

</div>
