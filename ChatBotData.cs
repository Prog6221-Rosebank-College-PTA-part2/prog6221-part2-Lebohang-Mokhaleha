using System;
using System.Collections.Generic;

namespace CyberSecurityChatBotWithUI
{
    static class ChatBotData
    {
        public static readonly List<string> Greetings = new List<string>
        {
            "Hello! I'm your CyberSecurity Awareness Bot. How can I help keep you safe online today?",
            "Welcome! Cybersecurity starts with awareness. What would you like to learn about?",
            "Hi there! Staying safe online is important. I'm here to help — ask me anything!",
            "Greetings! Whether it's phishing, passwords, or privacy — I've got you covered.",
            "Hey! Let's talk cybersecurity. No question is too small when it comes to your safety.",
        };

        public static readonly Dictionary<int, string> MenuResponses = new Dictionary<int, string>
        {
            {
                1,
                "HOW TO DETECT A PHISHING EMAIL:\n" +
                "Phishing emails trick you into revealing personal information.\n" +
                "Watch out for these red flags:\n" +
                "  - Urgent or threatening language (e.g. 'Your account will be closed!')\n" +
                "  - Sender address doesn't match the organisation (e.g. support@paypa1.com)\n" +
                "  - Generic greetings like 'Dear Customer' instead of your name\n" +
                "  - Suspicious links — hover before you click to see the real URL\n" +
                "  - Unexpected attachments, especially .exe, .zip, or .docm files\n" +
                "  - Requests for passwords, card numbers, or OTPs via email\n" +
                "TIP: When in doubt, go directly to the company's official website instead of clicking links."
            },
            {
                2,
                "HOW TO SECURE YOUR ACCOUNTS:\n" +
                "  - Use strong, unique passwords for every account (at least 12 characters)\n" +
                "  - Use a password manager (e.g. Bitwarden, 1Password) — never reuse passwords\n" +
                "  - Enable Two-Factor Authentication (2FA) on all accounts that support it\n" +
                "  - Prefer authenticator apps (e.g. Google Authenticator) over SMS 2FA\n" +
                "  - Regularly review app permissions and revoke access you no longer need\n" +
                "  - Set up account recovery options with a secure backup email or phone number\n" +
                "TIP: A compromised password on one site can lead to a credential stuffing attack on others."
            },
            {
                3,
                "WHAT TO DO IF YOU SUSPECT A DATA BREACH:\n" +
                "  1. Change your passwords immediately — start with email and banking accounts\n" +
                "  2. Enable 2FA on affected accounts if not already done\n" +
                "  3. Check HaveIBeenPwned.com to see if your email was exposed\n" +
                "  4. Monitor your bank statements for unauthorised transactions\n" +
                "  5. Alert your bank if financial information may have been compromised\n" +
                "  6. Report the breach to relevant authorities\n" +
                "  7. Freeze your credit if identity theft is suspected\n" +
                "TIP: Act fast — the sooner you respond, the less damage a breach can cause."
            },
            {
                4,
                "HOW TO KNOW IF A WEBSITE IS SECURE:\n" +
                "  - Look for HTTPS in the URL (the padlock icon in your browser)\n" +
                "  - Check the domain carefully for typos (e.g. amaz0n.com vs amazon.com)\n" +
                "  - Avoid entering personal info on sites with 'Not Secure' warnings\n" +
                "  - Use tools like Google Safe Browsing or VirusTotal to scan suspicious URLs\n" +
                "  - Legitimate businesses have clear contact info, privacy policies, and T&Cs\n" +
                "TIP: HTTPS means the connection is encrypted, but it does NOT mean the site itself is trustworthy."
            },
            {
                5,
                "WHAT IS RANSOMWARE?\n" +
                "Ransomware is malicious software that encrypts your files and demands payment\n" +
                "(usually in cryptocurrency) to restore access.\n\n" +
                "How to protect yourself:\n" +
                "  - Keep regular backups on an offline or cloud drive (3-2-1 backup rule)\n" +
                "  - Keep your OS and software up to date\n" +
                "  - Don't open email attachments from unknown senders\n" +
                "  - Use reputable antivirus/anti-malware software\n" +
                "  - Avoid downloading software from unofficial sources\n" +
                "TIP: If infected, DO NOT pay the ransom. There is no guarantee your files will be restored."
            },
            {
                6,
                "You selected 'Other'. Type a keyword below to ask a specific question.\n" +
                "Try: vpn, malware, firewall, password, wifi, social engineering,\n" +
                "two factor, cookies, encryption, privacy, scam, identity theft, and more!"
            }
        };

        public static readonly List<(string[] Keywords, string Response)> KeywordResponses =
            new List<(string[], string)>
        {
            (new[]{"vpn"},
            "VPN (Virtual Private Network):\n" +
            "A VPN encrypts your internet traffic and masks your IP address.\n" +
            "Use a trusted VPN on public Wi-Fi. Avoid free VPNs — they often sell your data."),

            (new[]{"malware"},
            "Malware (Malicious Software):\n" +
            "Malware is any software designed to harm your device or steal data.\n" +
            "Types include: viruses, worms, ransomware, spyware, adware, and trojans.\n" +
            "Always use updated antivirus software and avoid clicking unknown links."),

            (new[]{"firewall"},
            "Firewalls:\n" +
            "A firewall monitors and controls incoming/outgoing network traffic.\n" +
            "It acts as a barrier between your trusted network and untrusted ones.\n" +
            "Enable your OS firewall and consider a hardware firewall for your home network."),

            (new[]{"two factor","2fa","multi factor","mfa"},
            "Two-Factor Authentication (2FA / MFA):\n" +
            "2FA adds a second verification step beyond your password.\n" +
            "Even if your password is stolen, 2FA blocks unauthorised access.\n" +
            "Use an authenticator app (Google Authenticator, Authy) rather than SMS where possible."),

            (new[]{"password manager"},
            "Password Managers:\n" +
            "A password manager securely stores and generates strong, unique passwords.\n" +
            "Recommended: Bitwarden (free & open-source), 1Password, Dashlane.\n" +
            "You only need to remember one master password — make it long and memorable."),

            (new[]{"social engineering"},
            "Social Engineering:\n" +
            "Manipulates people into revealing confidential information.\n" +
            "Common tactics: phishing, pretexting, baiting, and tailgating.\n" +
            "Always verify identities before sharing any sensitive information."),

            (new[]{"public wifi","public wi-fi","open wifi"},
            "Public Wi-Fi Safety:\n" +
            "Public Wi-Fi is often unsecured and vulnerable to interception.\n" +
            "Avoid accessing banking or sensitive accounts on public Wi-Fi.\n" +
            "Always use a VPN when connecting to public networks."),

            (new[]{"encryption"},
            "Encryption:\n" +
            "Encryption scrambles data so only authorised parties can read it.\n" +
            "Use apps with end-to-end encryption (e.g. Signal).\n" +
            "Enable full-disk encryption on your devices (BitLocker / FileVault)."),

            (new[]{"cookie","cookies"},
            "Browser Cookies:\n" +
            "Cookies store small files on your browser to remember you.\n" +
            "Tracking cookies can follow you across websites for ad profiling.\n" +
            "Regularly clear cookies and use a browser extension like uBlock Origin."),

            (new[]{"privacy","online privacy"},
            "Online Privacy:\n" +
            "  - Use privacy-focused browsers (Firefox, Brave) and search engines (DuckDuckGo)\n" +
            "  - Review app permissions on your phone regularly\n" +
            "  - Limit personal info shared on social media\n" +
            "  - Read privacy policies before signing up for services"),

            (new[]{"identity theft"},
            "Identity Theft:\n" +
            "Occurs when someone steals your personal info to commit fraud.\n" +
            "  - Never share your ID number or bank details online\n" +
            "  - Monitor your credit report regularly\n" +
            "  - Shred physical documents containing personal info"),

            (new[]{"scam","online scam"},
            "Online Scams:\n" +
            "Common scams: lottery, romance, tech support, and job scams.\n" +
            "Red flags: unsolicited contact, payment requests, urgency, too-good-to-be-true offers.\n" +
            "Never send money or gift cards to someone you haven't met in person."),

            (new[]{"spyware"},
            "Spyware:\n" +
            "Secretly monitors your activity and sends data to a third party.\n" +
            "It can capture keystrokes, screenshots, and login credentials.\n" +
            "Use anti-spyware tools and avoid apps from unknown sources."),

            (new[]{"trojan","trojan horse"},
            "Trojan Horse Malware:\n" +
            "Disguises itself as legitimate software but carries malicious code.\n" +
            "Once installed, it can create backdoors or steal data.\n" +
            "Only download software from official, trusted sources."),

            (new[]{"keylogger"},
            "Keyloggers:\n" +
            "Records every keystroke you make, capturing passwords and sensitive info.\n" +
            "Can be hardware (physical) or software-based.\n" +
            "Keep your antivirus updated and use on-screen keyboards for sensitive logins."),

            (new[]{"ddos","denial of service"},
            "DDoS (Distributed Denial of Service) Attacks:\n" +
            "Floods a server with traffic to make it unavailable to users.\n" +
            "Organisations and websites are the typical targets.\n" +
            "Use DDoS protection services (e.g. Cloudflare) if you run a website."),

            (new[]{"zero day","zero-day"},
            "Zero-Day Vulnerabilities:\n" +
            "A security flaw unknown to the software vendor, giving hackers a head start.\n" +
            "Keep software updated — patches address newly discovered vulnerabilities.\n" +
            "Use security software with behavioural detection, not just signature-based scanning."),

            (new[]{"patch","software update","update"},
            "Software Updates & Patching:\n" +
            "Updates fix security vulnerabilities that hackers exploit.\n" +
            "Enable automatic updates for your OS, browser, and apps.\n" +
            "Unpatched software is one of the leading causes of successful cyberattacks."),

            (new[]{"backup"},
            "Data Backups:\n" +
            "Follow the 3-2-1 rule: 3 copies, on 2 different media, with 1 offsite.\n" +
            "Use cloud backups AND an external hard drive.\n" +
            "Test your backups regularly to ensure they can be restored."),

            (new[]{"dark web"},
            "The Dark Web:\n" +
            "A hidden part of the internet not indexed by search engines.\n" +
            "Your data may be sold on dark web marketplaces after a breach.\n" +
            "Check HaveIBeenPwned.com to see if your info has been exposed."),

            (new[]{"man in the middle","mitm"},
            "Man-in-the-Middle (MitM) Attacks:\n" +
            "A hacker intercepts communication between two parties.\n" +
            "Common on unsecured Wi-Fi networks.\n" +
            "Use HTTPS websites, a VPN, and avoid public Wi-Fi for sensitive transactions."),

            (new[]{"brute force"},
            "Brute Force Attacks:\n" +
            "Attackers try thousands of password combinations until they find the right one.\n" +
            "Protect yourself with long passwords, account lockout policies, and 2FA.\n" +
            "A 12+ character mixed password is exponentially harder to crack."),

            (new[]{"credential stuffing"},
            "Credential Stuffing:\n" +
            "Hackers use leaked username/password combos to access other accounts.\n" +
            "This works because many people reuse passwords across sites.\n" +
            "Use a unique password for every account — a password manager makes this easy."),

            (new[]{"pharming"},
            "Pharming:\n" +
            "Redirects users from a legitimate website to a fake one without their knowledge.\n" +
            "Can occur through DNS poisoning or malware on your device.\n" +
            "Always verify the URL in your browser and use a secure DNS (e.g. 1.1.1.1)."),

            (new[]{"vishing","voice phishing"},
            "Vishing (Voice Phishing):\n" +
            "Phishing carried out over phone calls.\n" +
            "Scammers impersonate banks, government agencies, or tech support.\n" +
            "Never give out personal info over the phone unless YOU initiated the call."),

            (new[]{"smishing","sms phishing"},
            "Smishing (SMS Phishing):\n" +
            "Fraudulent text messages with malicious links or fake alerts.\n" +
            "Never click links in unexpected texts — go to the official website instead.\n" +
            "Report smishing texts to your network provider."),

            (new[]{"insider threat"},
            "Insider Threats:\n" +
            "Comes from within an organisation — employees or contractors who misuse access.\n" +
            "Can be malicious or accidental.\n" +
            "Organisations should enforce least-privilege access and monitor unusual activity."),

            (new[]{"shoulder surfing"},
            "Shoulder Surfing:\n" +
            "Someone physically observes your screen or keyboard to steal information.\n" +
            "Use a privacy screen filter in public and be aware of your surroundings\n" +
            "when entering passwords or PINs."),

            (new[]{"bluetooth"},
            "Bluetooth Security:\n" +
            "Attackers can exploit Bluetooth to intercept data or send unsolicited messages.\n" +
            "Turn off Bluetooth when not in use.\n" +
            "Avoid pairing devices in public places."),

            (new[]{"iot","smart device","internet of things"},
            "IoT (Internet of Things) Security:\n" +
            "Smart devices are often insecure by default.\n" +
            "Always change default usernames and passwords and update firmware.\n" +
            "Put IoT devices on a separate guest network from your main devices."),

            (new[]{"antivirus","anti-virus"},
            "Antivirus & Anti-malware:\n" +
            "Use reputable software such as Windows Defender, Malwarebytes, or Bitdefender.\n" +
            "Keep definitions updated and run regular scans.\n" +
            "Combine antivirus with safe browsing habits for best results."),

            (new[]{"secure password","strong password"},
            "Creating Strong Passwords:\n" +
            "  - Use at least 12-16 characters\n" +
            "  - Mix uppercase, lowercase, numbers, and symbols\n" +
            "  - Avoid dictionary words, names, or dates\n" +
            "  - Use a passphrase e.g. Coffee@Sunrise7Rings\n" +
            "  - Never share your password with anyone"),

            (new[]{"https","ssl","tls"},
            "HTTPS / SSL / TLS:\n" +
            "HTTPS encrypts data between your browser and the server.\n" +
            "Always look for the padlock icon and https:// before entering information.\n" +
            "Never submit sensitive info on HTTP (non-secure) websites."),

            (new[]{"social media"},
            "Social Media Security:\n" +
            "  - Set profiles to private\n" +
            "  - Enable 2FA on all social media accounts\n" +
            "  - Be cautious about what personal info you share publicly\n" +
            "  - Beware of fake profiles, quizzes, and third-party app access"),

            (new[]{"email security","secure email"},
            "Email Security:\n" +
            "  - Be suspicious of unexpected attachments or links\n" +
            "  - Verify senders before replying to sensitive requests\n" +
            "  - Use encrypted email services like ProtonMail for sensitive communication\n" +
            "  - Enable spam filters and report phishing emails"),

            (new[]{"mobile security","phone security","smartphone"},
            "Mobile Device Security:\n" +
            "  - Lock your phone with a strong PIN or biometric\n" +
            "  - Only install apps from official stores\n" +
            "  - Review app permissions regularly\n" +
            "  - Enable remote wipe in case your phone is lost or stolen"),

            (new[]{"cloud security","cloud storage"},
            "Cloud Security:\n" +
            "  - Use strong, unique passwords for cloud accounts\n" +
            "  - Enable 2FA on Google Drive, OneDrive, Dropbox etc.\n" +
            "  - Avoid storing highly sensitive documents in the cloud\n" +
            "  - Review sharing settings to avoid accidental public access"),

            (new[]{"cyber hygiene","digital hygiene"},
            "Cyber Hygiene - Good Daily Habits:\n" +
            "  - Keep all software and OS updated\n" +
            "  - Use unique strong passwords with a password manager\n" +
            "  - Enable 2FA everywhere possible\n" +
            "  - Back up your data regularly\n" +
            "  - Think before you click any link or attachment"),

            (new[]{"home network","network security"},
            "Home Network Security:\n" +
            "  - Change your router's default admin username and password\n" +
            "  - Use WPA3 or WPA2 encryption on your Wi-Fi (never WEP)\n" +
            "  - Create a guest network for visitors and IoT devices\n" +
            "  - Regularly check connected devices for anything unfamiliar"),

            (new[]{"pentest","penetration test","ethical hacking"},
            "Penetration Testing & Ethical Hacking:\n" +
            "Authorised simulated attacks to find security weaknesses.\n" +
            "Ethical hackers help organisations fix vulnerabilities before attackers exploit them.\n" +
            "Certifications to look into: CompTIA Security+, CEH, OSCP."),

            (new[]{"cryptojacking"},
            "Cryptojacking:\n" +
            "Uses your device's processing power to mine cryptocurrency without consent.\n" +
            "Signs: slow device, high CPU usage, overheating.\n" +
            "Use an ad blocker and keep software updated to prevent it."),

            (new[]{"adware"},
            "Adware:\n" +
            "Displays unwanted ads on your device and may track your behaviour.\n" +
            "Often bundled with free software downloads.\n" +
            "Always choose Custom Install and deselect unwanted extras."),

            (new[]{"rootkit"},
            "Rootkits:\n" +
            "Gives attackers persistent hidden admin-level access to your system.\n" +
            "Very hard to detect as they hide from the OS.\n" +
            "Use bootable antivirus scanners and consider a full OS reinstall if infected."),

            (new[]{"worm"},
            "Computer Worms:\n" +
            "Self-replicating malware that spreads across networks without user interaction.\n" +
            "Famous examples: WannaCry, Conficker.\n" +
            "Keep systems patched and segment your network to limit worm propagation."),

            (new[]{"digital footprint"},
            "Your Digital Footprint:\n" +
            "The trail of data you leave online — posts, searches, purchases.\n" +
            "Reduce it by using privacy browsers, opting out of data collection,\n" +
            "and deleting old accounts you no longer use."),

            (new[]{"cybercrime","cyber law"},
            "Cybercrime & Cyber Law:\n" +
            "Cybercrime includes hacking, identity theft, fraud, and cyberbullying.\n" +
            "In South Africa, the Cybercrimes Act (No. 19 of 2020) criminalises these offences.\n" +
            "Report cybercrime to SAPS or visit www.cybercrime.org.za"),

            (new[]{"dumpster diving"},
            "Dumpster Diving:\n" +
            "Attackers retrieve sensitive documents from bins to gather personal info.\n" +
            "Always shred documents containing names, account numbers, or ID details.\n" +
            "This applies to old hard drives and USB drives too."),

            (new[]{"tor","darknet"},
            "Tor & The Darknet:\n" +
            "Tor anonymises internet traffic by routing it through multiple servers.\n" +
            "Used legitimately by journalists and privacy advocates.\n" +
            "Accessing illegal content on the darknet is a criminal offence."),

            (new[]{"sql injection"},
            "SQL Injection:\n" +
            "Exploits vulnerabilities in web forms to manipulate a database.\n" +
            "Mainly a concern for developers and website owners.\n" +
            "Use parameterised queries, input validation, and web application firewalls."),

            (new[]{"awareness","training","cyber awareness"},
            "Cybersecurity Awareness:\n" +
            "Human error is the number one cause of security breaches.\n" +
            "Regular training helps people recognise threats like phishing and scams.\n" +
            "Share what you know — awareness protects everyone around you too."),
        };

        public static string GetRandomGreeting()
        {
            Random rng = new Random();
            return Greetings[rng.Next(Greetings.Count)];
        }

        public static string? FindKeywordResponse(string input)
        {
            string lower = input.ToLower();
            foreach (var (keywords, response) in KeywordResponses)
            {
                foreach (string kw in keywords)
                {
                    if (lower.Contains(kw))
                        return response;
                }
            }
            return null;
        }
    }
}
