# 🧠 Cipher - Interactive Cybersecurity Chatbot & Task Assistant

Cipher is a desktop chatbot application built using WPF (XAML) and C# designed to bridge the gap in basic cybersecurity awareness. Developed as an interactive educational tool, Cipher aims to counteract modern, increasing security threats (such as phishing, credential theft, and social engineering) by teaching users foundational security habits and helping them track their personal security tasks.
🎯 Project Scenario & Purpose

In an era where cyber threats are rapidly expanding due to generative AI and sophisticated social engineering tactics, the average internet user remains highly vulnerable. Most people are unaware of the day-to-day digital risks they face.

Cipher was built to solve this problem by:

* Educating: Delivering interactive, easy-to-understand definitions and mitigation tactics for concepts like Hacking, Phishing, and Safe Browsing.

* Testing: Challenging users via a dedicated multiple-choice quiz interface to reinforce learning.

* Acting: Working as a personalized Task Assistant backed by a MySQL database to help users organize, schedule, and review critical cybersecurity habits (like setting up 2FA or auditing privacy settings).

✨ Features
💬 Core Chatbot Engine (Cipher)

* Dynamic Content Routing: Implements rule-based intent mapping to provide relevant security definitions, real-time advice, and actionable guidance based on user questions.

* Randomized Tip Delivery: Uses a randomized selection pool to deliver fresh, diverse cybersecurity tips when users ask about safe web browsing, avoiding repetitive responses.

🧠 Dedicated Quiz Framework

* Gamified Learning: Users can launch a dedicated multiple-choice interface to test their cybersecurity knowledge.

* State Management Navigation: Smoothly switches from the main conversation view into a completely separate XAML evaluation frame, tracking progress and score before automatically returning to the chat.

📋 Database-Backed Task Storage (MySQL Integration)

* Automated Persistence: Connects to a local MySQL engine to natively store custom security tasks.

* Command-Driven Management: Allows users to interact with their data seamlessly via regular chat commands:

    * view tasks — Lists all pending tasks, descriptions, and custom alerts.

    * add task: Title | Description | ReminderDays — Schedules a new cybersecurity task.

    * delete task: ID — Removes or clears tasks upon completion.

🛠️ Built With

* Frontend: XAML (WPF - Windows Presentation Foundation)

* Backend: C# (.NET 8.0 / .NET Framework)

* Database: MySQL (via MySql.Data Connector)

* IDE: Visual Studio 2022

🚀 Getting Started
Prerequisites

* Visual Studio 2022 with the .NET Desktop Development workload installed.

* A local MySQL server instance running (via XAMPP, WAMP, or standard MySQL Installer).

Database Configuration

Before running the application, make sure your connection parameters are accurate inside DatabaseHelper.cs:
C#

private static string connectionString = "server=127.0.0.1;port=3306;database=cyber_tasks;user=root;password=YOUR_PASSWORD;";

Note: The application will automatically create the cyber_tasks database and the underlying tracking tables on its very first launch if they don't already exist.
Setup Instructions


* Open the .sln solution file in Visual Studio 2022.

* Restore any missing NuGet packages (MySql.Data).
* Press F5 or click Start to build and run the application!
