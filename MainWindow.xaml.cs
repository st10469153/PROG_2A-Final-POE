using System.Collections.ObjectModel;
using System.Net.Mail;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading;
using System;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Navigation; 
using Microsoft.VisualBasic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using ChatBotApp;


namespace ChatBotApp
{

    public partial class MainWindow : Window
    {
        // Collection that automatically updates the XAML UI when items are added
        public ObservableCollection<ChatMessage> Messages { get; set; }
        private Random _randomizer = new Random();
        private string userName = "User";

        // Keep a reference so the player isn't GC'd while playing
        private MediaPlayer _startupPlayer;


        public MainWindow()
        {
            InitializeComponent();
            Messages = new ObservableCollection<ChatMessage>();
            ChatHistoryItems.ItemsSource = Messages;
            string name = getUserName();

            // Play a short startup sound (see note below about adding the file)
            PlayStartupSound();

            // Initialize Database
            try { DatabaseHelper.InitializeDatabase(); }
            catch (Exception ex) { MessageBox.Show("Failed to connect to MySQL: " + ex.Message); }

            Messages = new ObservableCollection<ChatMessage>();
            ChatHistoryItems.ItemsSource = Messages;

            // Welcome Message
            Messages.Add(new ChatMessage("Hello " + name + "!\n What would you like to know Today? ", false));


        }

        private string getUserName()
        {
            userName = Interaction.InputBox("What is your name?");
            //converts name varible to titlecase e.g. thando -> Thando
            userName= char.ToUpper(userName[0]) + userName.Substring(1).ToLower();

            if (userName == null || userName == ""){ MessageBox.Show("Please enter your name to continue"); 
                userName = Interaction.InputBox("What is your name?");
                //converts name varible to titlecase e.g. thando -> Thando
                userName = char.ToUpper(userName[0]) + userName.Substring(1).ToLower();
            }

            MessageBox.Show("Hello " + userName + ".\nTo interact with Cipher, you will use the chat box provided");
            return userName;
        }

        private void PlayStartupSound()
        {
            try
            {
                // Place a file named "startup.mp3" (or .wav) in the output folder (project root / Resources and set Copy to Output Directory)
                string filePath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "bootUpSound.wav");
                if (!System.IO.File.Exists(filePath))
                    return;

                _startupPlayer = new MediaPlayer();
                _startupPlayer.Open(new Uri(filePath, UriKind.Absolute));
                _startupPlayer.Volume = 0.6; // adjust volume 0.0 - 1.0
                _startupPlayer.Play();
            }
            catch
            {
                // swallow exceptions here to avoid breaking app startup if audio fails
            }
        }
        private void SendButton_Click(object sender, RoutedEventArgs e)
        {
            SendMessage();

        }

        private void InputTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            // Allow sending by hitting the Enter key
            if (e.Key == Key.Enter)
            {
                SendMessage();
            }
        }

        

        private async void SendMessage()
        {
            string userText = InputTextBox.Text.Trim();
            //handling empty input
            if (string.IsNullOrEmpty(userText)) {
                MessageBox.Show("Please type something in the text box before hitting send");
                return;
            }
        

            // 1. Append User Message (Right-aligned, Blueish tint)
            Messages.Add(new ChatMessage(userText, isUser: true));
            InputTextBox.Clear();
            ScrollToBottom();

            // 2. Simulate Bot Response (Left-aligned, Gray tint)
            string botReply = GetBotResponse(userText);

            // Bot output delay for interactivity
            await Task.Delay(500);

            Messages.Add(new ChatMessage(botReply, isUser: false));
            ScrollToBottom();

            
        }

        private string GetBotResponse(string userInput)
        {
            // Convert to lowercase so the matching isn't case-sensitive (e.g., "Hi" vs "hi")
            string cleanedInput = userInput.ToLower().Trim();

            // Rule 2: about the bot
            if (cleanedInput.Contains("what do you do") || cleanedInput.Contains("what can you do")  || cleanedInput.Contains("purpose") || cleanedInput.Contains("what is your purpose") || cleanedInput.Contains("what do you do") || cleanedInput.Contains("what are you") || cleanedInput.Contains("what is your duty") || cleanedInput.Contains("who are you") || cleanedInput.Equals("what is your job"))
            {
                return "I am Cipher.\nI am a chatbot focused to educate you about cybersecurity and tech-related security measures. You can ask me questions related to cybersecurity and internet safety"+
                    "I was made in 2026 as an attempt to counter the increasing number of cyberattacks that were taking place." +
                    "Since most people are not as aware about the potential dangers and risks that come with technology, there are people who take exploit that to their own benefit" +
                    "So if you are eager to learn, go ahead and ask any cyberesecurity-related questions you may be qurious about"
                    + "\nYou can even attemt a quiz if you want to test your knowledge. Just ask";
            }

            // cybersecurity
            if (cleanedInput.Equals("what is cybersecurity") || cleanedInput.Contains("cybersecurity"))
                { 
                return "In basic terms, Cybersecurity is the practice of protecting systems, networks, and programs from digital attacks, commonly called cyberattacks or hacking.\n" +
                    "Think of cybersecurity as the digital versions of superheroes who stop the villains who try to rob banks.\n But in this case, the villains are doing it through the internet, and no one is held at gunpoint. " +
                    "These cyberattacks are usually aimed at accessing, modifying, or destroying restricted sensitive information; extorting money from users through ransomware, or interrupting normal business processes.\n" +
                    "Now we can try to prevent cyberattacks by implementing effective cybersecurity techniques." +
                    " Effective techniques can work the best when they prevent damage to critical before they are even attacked, thus keeping all systems running at 100% \n So that's it :)";
            }
            // Rule 3: Hacking definition
            if (cleanedInput.Contains("what is hacking") || cleanedInput.Contains("hacking"))
            {
                return "Hacking is a term used to explain an illegal digital attack on systems.It is the foundation that cybersecurity works towards fighting";
            }

            // Rule 4: Hacked measures
            if (cleanedInput.Contains("hacked") || cleanedInput.Contains("hack"))
            {
                return userName + ". If you believe you have been exposed or potentially given away sensitive information, you should hurry to protect your accounts.\n"
                    + "The reason for hurrying is because if an attacker got access to your account, it is possible that they can log YOU out of your own account.\n"
                    + "It is therefore crucial that you act fast if this happens:\n"
                    + "Change your passwords\n" 
                    + "Sign out on all devices that has and may have access to your account\n" +
                            "If you may have given away banking information, block your cards and ask the bank for new ones.\n" +
                            "This should prevent a lot of damage which could be critical to your assets/personal information but it doesn't guarantee that you will be fully protected";
                ;
            }
            //phishing
            if(cleanedInput.Contains("phishing") || cleanedInput.Contains("phish") || cleanedInput.Contains("what is phishing"))
            {
                return "So phishing is a dangerous tactic used by attackers to trick users to granting unauthorized access to sensitive information\n" +
                    "They do this by posing as a legitimate and professional organization.\n" +
                     "The messages are personalized and get increasingly difficult to distinguish from the legit organization.\n" +
                            "Today, users have become more prone to phishing attacks as generative AI is able to make super realistic website/applications that look just as good as the original.\n" +
                            "Unfortunately, this issue is bound to get worse as AI continuously evolves.";
            }
            // Rule 5: goodbye
            if (cleanedInput.Contains("bye") || cleanedInput.Contains("goodbye") || cleanedInput.Contains("exit"))
            {
                return "Goodbye! Have a fantastic day!";
                Close();

                //System.Windows.Application.Current.Shutdown();
            }
            //safe internet browsing
            if (cleanedInput.Contains("safe") || cleanedInput.Contains("browsing") || cleanedInput.Contains("safety") || cleanedInput.Contains("internet safety") )
            {
                string[] safetyTips = new string[]
                {
                    "Be cautious with links and attachments\n" +
                            "A new and arising dangerous attack used today is link redirection.\n" +
                            "You click on a link that is intended to take you to your planned website, but it redirects you to a fake similar webpage\n" +
                            "More often, users cannot tell the difference, so they enter their personal information.\n" +
                            "Verify the link before you click on it and check if it says that it will take you to where you want it to\n" +
                            "You could also use an online link verifier to ensure that it is safe to click.\n" +
                            "Remember the phrase:\nthink before you click"
                            ,

                    "Use strong unique passwords\n" +
                            "As the everyday systems we use develop, we are encouraged to come up with strong passwords that would be difficult to guess or predict\n" +
                             "Even so, you could still be at risk." +
                             "As unique as your password may be, you shouldn’t reuse the same one across sites." +
                             "If one were to get their hands on it, they would have access to all the other sites with all your personal information"
                            ,

                    "Limit the amount of personal information you share online\n" +
                             "As you explore various platforms, you will find yourself encouraged to share personal details to further improve your user experience\n" +
                             "Many don’t realize how much information is being collected over time" +
                             "Most already try their best to protect their data but regardless, companies still try to obtain some level of data from users" 
                             ,

                    "Recognize emotional manipulation online\n" +
                             "A lot of online threats, commonly scams rely on the psychological sense of urgency, fear, or excitement to persuade users to perform quick reactions to satisfy those emotions." +
                             "Examples of this could be a bank telling you that your account was potentially hacked and requires immediate attention." +
                             "It is best advised to take a moment to think before you click or respond in order to break that psychological pattern." +
                             "Slowing down is the most effective defences against manipulation-driven attacks."
                            ,

                    "Treat online safety as an ongoing habit\n" +
                             "You need to understand that cybersecurity is a process, a habit and not a single one-time setup that works forever.\n" +
                             " As technology evolves, so do security threats and precautions that worked perfectly a year ago may not be sufficient in today’s time.\n" +
                             "Try to stay informed about potential dangers, continue to perform basic practices, and apply pattern recognition as you use these daily systems.\n" +
                             "You would be surprised how many users have been a victim and how it’s not all just theory and risk."
                };

                int randomIndex = _randomizer.Next( safetyTips.Length );
            
            return safetyTips[randomIndex];
            }
            // Rule 6: Gratitude
            if (cleanedInput.Contains("thank you") || cleanedInput.Contains("thanks") || cleanedInput.Contains("appreciate"))
            {
                return "You are welcome!";
            }



            // Display Quiz
            if (cleanedInput.Contains("quiz") || cleanedInput.Contains("test") || cleanedInput.Contains("challenge"))
            {
                QuizPage quiz = new QuizPage();
                quiz.Show();

                return "Opening the quiz window now... Good luck!";
            }

            // Fallback: If the bot doesn't understand the intent
            return "Sorry, I do not understand the question.\nTry to rephrase the question or try questions related to cybersecurity or even about my purpose. \n" +
                "Here is a list of things you can ask here:\n" +
                "Questions related to me:\nMy purpose\nWho made me\nWhat I can do\n" +
                                        "\nCybersecurity-related questions:\n" +
                                        "what is cybersecurity?\nWhy is cybersecurity important?\nWhat is hacking?" +
                                        "Other security-related questions:\n" +
                                        "Safe Web Browsing\nWhat is phishing?\nPassword Safety";


            // DATABASE COMMANDS (for task management)

            // Command A: View Tasks (e.g., "view tasks", "show my tasks")
            if (cleanedInput.Contains("view tasks") || cleanedInput.Contains("show tasks") || cleanedInput == "tasks")
            {
                return DatabaseHelper.ViewTasks();
            }

            // Command B: Add Task via plain structure routing 
            // Format required: add task: Title | Description | Days
            if (cleanedInput.StartsWith("add task:"))
            {
                try
                {
                    // Extract content past the "add task:" identifier
                    string rawContent = userInput.Substring(9).Trim();
                    string[] parts = rawContent.Split('|');

                    if (parts.Length < 2)
                    {
                        return "⚠️ Invalid format! Please use: \nadd task: Title | Description | Optional Days\n\nExample:\nadd task: 2FA Setup | Enable two-factor authentication on banking profile | 7";
                    }

                    string title = parts[0].Trim();
                    string description = parts[1].Trim();
                    int? reminderDays = null;

                    if (parts.Length >= 3 && int.TryParse(parts[2].Trim(), out int parsedDays))
                    {
                        reminderDays = parsedDays;
                    }

                    return DatabaseHelper.AddTask(title, description, reminderDays);
                }
                catch
                {
                    return "❌ Something went wrong parsing that task creation command structure.";
                }
            }

            // Command C: Delete/Complete Task 
            // Format required: delete task: ID
            if (cleanedInput.StartsWith("delete task:") || cleanedInput.StartsWith("remove task:"))
            {
                string rawId = cleanedInput.Replace("delete task:", "").Replace("remove task:", "").Trim();
                if (int.TryParse(rawId, out int taskId))
                {
                    return DatabaseHelper.DeleteTask(taskId);
                }
                return "⚠️ Please specify a valid numeric task ID. Example: delete task: 3";
            }

            // Command A: View Tasks (e.g., "view tasks", "show my tasks")
            if (cleanedInput.Contains("view tasks") || cleanedInput.Contains("show tasks") || cleanedInput == "tasks")
            {
                return DatabaseHelper.ViewTasks();
            }
        }

        private void ScrollToBottom()
        {
            // Ensures the chat log always scrolls down to show latest text
            ChatScrollViewer.ScrollToEnd();
        }
    }

    // Simple Data Model to handle message presentation properties
    public class ChatMessage
    {
        public string MessageText { get; set; }
        public HorizontalAlignment Alignment { get; set; }
        public Brush BackgroundColor { get; set; }

        public ChatMessage(string text, bool isUser)
        {
            MessageText = text;

            if (isUser)
            {
                Alignment = HorizontalAlignment.Right;
                BackgroundColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#DED9FF")); // Light Blue
            }
            else
            {
                Alignment = HorizontalAlignment.Left;
                BackgroundColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#E0CC75")); // Yellow-ish
            }
        }
    }
}
