[README.md](https://github.com/user-attachments/files/29420671/README.md)
# CyberSecurity Awareness ChatBot

A Windows desktop chatbot built in **C# / WPF (XAML)** that educates users on
cybersecurity best practices through an interactive, menu-driven conversation.

---

## Features

| # | Feature | Description |
|---|---------|-------------|
| 1 | **Keyword Search** | Type any cybersecurity topic to get an instant answer. Covers phishing, malware, VPNs, ransomware, encryption, 2FA, and more. |
| 2 | **NLP Simulation** | Every message is analysed for sentiment (worried, positive, negative) and intent (greeting, thanks, urgent). The bot adapts its tone accordingly. |
| 3 | **Task Assistant** | Add, view, complete, and delete cybersecurity to-do tasks. Supports optional reminders. All tasks are saved to a local SQLite database and persist between sessions. |
| 4 | **Mini Quiz** | 5 random questions drawn from a bank of 10. Multiple choice (A/B/C/D). Instant feedback with explanations after each answer and a final score. |
| 5 | **Activity Log** | Every action is logged in-memory (last 10 entries) and to `activity_log.txt` on disk. Type `show activity log` at any time to view recent actions in chat. |

---

## Requirements

- Windows 10 or later
- [.NET 10 SDK](https://dotnet.microsoft.com/download) (or the version matching your project target)
- Visual Studio 2022 (recommended) **or** the `dotnet` CLI

No external database server is needed. SQLite is bundled via NuGet.

---

## Getting Started

**1. Clone or download the project**

**2. Restore NuGet packages**
```
dotnet restore
```

**3. Build and run**
```
dotnet run
```
Or open the `.sln` / `.csproj` in Visual Studio and press **F5**.

---

## Project Structure

```
CyberSecurityChatBotWithUI/
│
├── App.xaml                  # WPF application entry point
├── App.xaml.cs
│
├── MainWindow.xaml           # UI layout — chat window, input box, buttons
├── MainWindow.xaml.cs        # All conversation logic and state machine
│
├── ChatBotData.cs            # Keyword response dictionary + random greetings
├── TaskManager.cs            # SQLite database operations (add/view/complete/delete tasks)
├── QuizData.cs               # Quiz question bank and random shuffle logic
├── NlpHelper.cs              # Sentiment and intent detection
└── ActivityLog.cs            # In-memory recent log + persistent log file writer
```

---

## How to Use

**Starting a session**
Type your name and press Enter or click Send.

**Main menu options**
```
1. Search by keyword     — ask about any cybersecurity topic
2. Task Assistant        — manage your cybersecurity to-do list
3. Mini Quiz             — test your knowledge (5 random questions)
4. Exit                  — end the session
```

**Task Assistant commands** (shown after selecting option 2 → V)
```
DONE <id>   — mark a task as complete    e.g.  DONE 2
DEL  <id>   — permanently delete a task  e.g.  DEL 3
B           — go back to the task menu
```

**Activity log** — works at any point during a session:
```
show activity log
show log
what have you done for me?
```

**Continue prompt** — after the bot answers, type:
```
Y   — go back to the main menu
N   — say goodbye and end the session
```

---

## Generated Files

The app creates two files next to the executable when it first runs:

| File | Contents |
|------|----------|
| `cybertasks.db` | SQLite database storing all tasks |
| `activity_log.txt` | Full timestamped log of every session |

---

## Keyword Topics Supported

`phishing` · `scam` · `malware` · `virus` · `ransomware` · `firewall` · `vpn` ·
`password` · `two factor` · `2fa` · `encryption` · `backup` · `identity theft` ·
`social engineering` · `data breach` · `public wifi` · and more — see `ChatBotData.cs`

---

## Colour Scheme

| Colour | Hex | Used for |
|--------|-----|----------|
| Matrix green | `#00FF41` | Bot name tag `[CyberBot]` |
| Electric cyan | `#00BFFF` | User name tag |
| Alert orange | `#FF4500` | Section dividers |
| Light grey | `#C8C8C8` | All body text |
| Gold | `#FFD700` | Quiz incorrect-answer feedback |
