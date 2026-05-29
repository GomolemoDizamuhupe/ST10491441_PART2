# 🛡️ CyberBot — Cybersecurity Awareness Chatbot (Part 2)

**Student:** Gomolemo Dizamuhupe  
**Student Number:** ST10491441  
**Module:** Programming 2A (PROG6221)

---

## 📋 Table of Contents

- [About the Project](#about-the-project)
- [Features](#features)
- [Technologies Used](#technologies-used)
- [Getting Started](#getting-started)
  - [Prerequisites](#prerequisites)
  - [Installation](#installation)
  - [Running the Application](#running-the-application)
- [How to Use](#how-to-use)
- [Project Structure](#project-structure)
- [Video Presentation](#video-presentation)
- [Version History](#version-history)

---

## About the Project

CyberBot is a WPF-based Cybersecurity Awareness Chatbot designed to educate users on key online safety topics. Part 2 expands the original console application into a fully interactive GUI, adding keyword recognition, sentiment detection, memory/recall, random responses, and a natural conversation flow.

---

## Features

| Feature | Description |
|---|---|
| **GUI Interface** | WPF application with a welcome screen, username entry, and chat window |
| **Voice Greeting** | Plays an audio greeting (`Greeting.wav`) on launch |
| **Returning User Detection** | Recognises users who have chatted before via a local `Usernames.txt` file |
| **Keyword Recognition** | Responds to 8 cybersecurity topics: `password`, `phishing`, `malware`, `2FA`, `ransomware`, `safe browsing`, `privacy`, `scam` |
| **Random Responses** | Randomly selects from 3 tips per topic, never repeating the same tip twice in a row |
| **Sentiment Detection** | Detects `worried`, `curious`, `frustrated`, and `happy` tones and adjusts responses empathetically |
| **Memory & Recall** | Remembers the user's name and favourite topic, referencing them later in conversation |
| **Interest Tracking** | Notices when a user repeatedly asks about the same topic and comments on it |
| **Follow-up Handling** | Handles follow-up prompts like "explain more", "another tip", or "tell me more" |
| **Input Validation** | Rejects usernames containing numbers or special characters |
| **Error Handling** | Default response for unrecognised inputs without crashing |

---

## Technologies Used

- **Language:** C# (.NET Framework)
- **UI Framework:** WPF (Windows Presentation Foundation)
- **Data Structures:** `Dictionary<string, List<string>>`, `List<string>`, `HashSet<string>`
- **Audio:** `System.Media.SoundPlayer`
- **Pattern Matching:** `System.Text.RegularExpressions`
- **File I/O:** `System.IO.File` for user persistence

---

## Getting Started

### Prerequisites

- Windows OS
- [Visual Studio 2019 or later](https://visualstudio.microsoft.com/) with the **.NET desktop development** workload installed
- .NET Framework 4.7.2 or later

### Installation

1. Clone the repository:
   ```bash
   git clone https://github.com/GomolemoDizamuhupe/ST10491441_PART2.git
   ```
2. Open the solution file in Visual Studio:
   ```
   PART2(POE).sln
   ```
3. Ensure `Greeting.wav` and `logo.png` are present in the project root and their **Build Action** is set to `Content` and **Copy to Output Directory** is set to `Copy if newer`.

### Running the Application

1. Press **F5** or click **Start** in Visual Studio.
2. The application will launch and play the voice greeting automatically.
3. Click **Start ChatBot** to begin.

---

## How to Use

1. **Welcome screen** — Click `Start ChatBot` to proceed.
2. **Enter your name** — Type your name (letters only) and click `Submit`.
3. **Chat** — Type any of the following to get started:

| What you can ask | Example |
|---|---|
| A cybersecurity topic | `"Tell me about phishing"` |
| Your favourite topic | `"I'm interested in privacy"` |
| How you feel about a topic | `"I'm worried about scams"` |
| Follow-up questions | `"Tell me more"` / `"Another tip"` |
| General questions | `"How are you?"` / `"What can I ask?"` |

4. Click **Send** or press the button to submit your message.
5. Click **Exit** to close the application.

---

## Project Structure

```
ST10491441_PART2/
├── MainWindow.xaml          # UI layout (WPF XAML)
├── MainWindow.xaml.cs       # UI logic and event handlers
├── bot.cs                   # Core chatbot logic (keyword recognition, sentiment, memory)
├── SoundGreet.cs            # Voice greeting handler
├── App.xaml / App.xaml.cs   # Application entry point
├── logo.png                 # Chatbot logo displayed in the GUI
├── Greeting.wav             # Voice greeting audio file
├── Usernames.txt            # Auto-generated file storing returning user names
├── PART2(POE).csproj        # Project file
└── README.md                # This file
```

---

## Video Presentation

A full walkthrough of the project, including code explanation and feature demonstration, is available here:

🎥 **[Watch on YouTube](https://youtu.be/b85dcPDUQZA)**

---

## Version History

### v1.2 — GUI & Feature Expansion
- Added WPF GUI with multi-screen flow (welcome → username → chat)
- Implemented voice greeting on launch
- Added returning user detection via file persistence
- Introduced sentiment detection (worried, curious, frustrated, happy)
- Added favourite topic memory and interest tracking
- Implemented follow-up question handling

### v1.1 — Initial Release
- Console-based cybersecurity chatbot
- Keyword recognition for 8 topics
- Random response selection per topic
- Basic username input and validation
