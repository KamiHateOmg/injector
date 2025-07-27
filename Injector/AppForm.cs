using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace Injector
{
    public partial class AppForm : Form
    {
        private Panel mainContainer;
        private Panel headerPanel;
        private Panel gameCardPanel;
        private Panel actionPanel;
        private Label titleLabel;
        private Label subtitleLabel;
        private PictureBox gameIcon;
        private Label gameTitle;
        private Label gameStatus;
        private Button injectButton;
        private Panel progressContainer;
        private Panel progressBar;
        private Label progressText;
        private Panel statusDot;
        private Button minimizeButton;
        private Button closeButton;
        private Panel backgroundEffect;

        private bool isGameDetected = false;
        private bool isInjected = false;
        private bool isInjecting = false;
        private float progressValue = 0f;
        private Timer animationTimer;
        private float pulseAnimation = 0f;
        private float glowAnimation = 0f;
        private float backgroundParticles = 0f;
        private Dictionary<Control, float> hoverAnimations = new Dictionary<Control, float>();
        private Dictionary<Control, bool> isHovered = new Dictionary<Control, bool>();

        public AppForm()
        {
            InitializeComponent();
            SetupPremiumUI();
            StartAdvancedAnimations();
            CheckGameStatus();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            this.AutoScaleDimensions = new SizeF(8F, 16F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(520, 360);
            this.Name = "AppForm";
            this.Text = "Nexus Injector";
            this.ResumeLayout(false);
        }

        private void SetupPremiumUI()
        {
            // Form setup with premium styling
            this.Size = new Size(520, 360);
            this.FormBorderStyle = FormBorderStyle.None;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(10, 10, 15);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint |
                         ControlStyles.DoubleBuffer | ControlStyles.ResizeRedraw, true);

            // Background effect panel for particles/gradients
            backgroundEffect = new Panel()
            {
                Size = this.Size,
                Location = new Point(0, 0),
                BackColor = Color.Transparent
            };
            backgroundEffect.Paint += BackgroundEffect_Paint;
            this.Controls.Add(backgroundEffect);

            // Main container with advanced glassmorphism
            mainContainer = new Panel()
            {
                Size = new Size(480, 320),
                Location = new Point(20, 20),
                BackColor = Color.Transparent
            };
            mainContainer.Paint += MainContainer_Paint;
            this.Controls.Add(mainContainer);

            SetupHeader();
            SetupGameCard();
            SetupActionPanel();
            SetupAnimationTracking();

            // Enable premium dragging
            headerPanel.MouseDown += Form_MouseDown;
            titleLabel.MouseDown += Form_MouseDown;
            subtitleLabel.MouseDown += Form_MouseDown;
        }

        private void SetupHeader()
        {
            headerPanel = new Panel()
            {
                Size = new Size(480, 70),
                Location = new Point(0, 0),
                BackColor = Color.Transparent
            };
            headerPanel.Paint += HeaderPanel_Paint;

            titleLabel = new Label()
            {
                Text = "NEXUS",
                Font = new Font("Segoe UI", 18F, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.Transparent,
                Location = new Point(30, 18),
                Size = new Size(120, 32)
            };

            subtitleLabel = new Label()
            {
                Text = "Premium Game Injector",
                Font = new Font("Segoe UI", 10F, FontStyle.Regular),
                ForeColor = Color.FromArgb(160, 170, 180),
                BackColor = Color.Transparent,
                Location = new Point(30, 45),
                Size = new Size(200, 20)
            };

            // Premium window controls
            minimizeButton = CreateWindowButton("─", new Point(410, 20));
            minimizeButton.Click += (s, e) => this.WindowState = FormWindowState.Minimized;

            closeButton = CreateWindowButton("✕", new Point(445, 20));
            closeButton.Click += (s, e) => this.Close();
            closeButton.Tag = "close"; // Special styling for close button

            headerPanel.Controls.AddRange(new Control[] { titleLabel, subtitleLabel, minimizeButton, closeButton });
            mainContainer.Controls.Add(headerPanel);
        }

        private Button CreateWindowButton(string text, Point location)
        {
            var button = new Button()
            {
                Text = text,
                Size = new Size(28, 28),
                Location = location,
                BackColor = Color.Transparent,
                ForeColor = Color.FromArgb(180, 180, 180),
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", text == "─" ? 12F : 9F, FontStyle.Bold),
                Cursor = Cursors.Hand
            };

            button.FlatAppearance.BorderSize = 0;
            button.Paint += WindowButton_Paint;

            hoverAnimations[button] = 0f;
            isHovered[button] = false;

            button.MouseEnter += (s, e) => { isHovered[button] = true; };
            button.MouseLeave += (s, e) => { isHovered[button] = false; };

            return button;
        }

        private void SetupGameCard()
        {
            gameCardPanel = new Panel()
            {
                Size = new Size(420, 130),
                Location = new Point(30, 90),
                BackColor = Color.Transparent,
                Cursor = Cursors.Hand
            };
            gameCardPanel.Paint += GameCardPanel_Paint;
            gameCardPanel.Click += GameCard_Click;

            hoverAnimations[gameCardPanel] = 0f;
            isHovered[gameCardPanel] = false;
            gameCardPanel.MouseEnter += (s, e) => { isHovered[gameCardPanel] = true; };
            gameCardPanel.MouseLeave += (s, e) => { isHovered[gameCardPanel] = false; };

            // Premium game icon
            gameIcon = new PictureBox()
            {
                Size = new Size(70, 70),
                Location = new Point(30, 30),
                BackColor = Color.Transparent,
                Cursor = Cursors.Hand
            };
            gameIcon.Paint += GameIcon_Paint;
            gameIcon.Click += GameCard_Click;

            gameTitle = new Label()
            {
                Text = "Counter-Strike 2",
                Font = new Font("Segoe UI", 16F, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.Transparent,
                Location = new Point(120, 40),
                Size = new Size(220, 28),
                Cursor = Cursors.Hand
            };
            gameTitle.Click += GameCard_Click;

            gameStatus = new Label()
            {
                Text = "Scanning for game...",
                Font = new Font("Segoe UI", 11F),
                ForeColor = Color.FromArgb(140, 150, 160),
                BackColor = Color.Transparent,
                Location = new Point(120, 70),
                Size = new Size(250, 22),
                Cursor = Cursors.Hand
            };
            gameStatus.Click += GameCard_Click;

            statusDot = new Panel()
            {
                Size = new Size(16, 16),
                Location = new Point(380, 52),
                BackColor = Color.Transparent,
                Cursor = Cursors.Hand
            };
            statusDot.Paint += StatusDot_Paint;
            statusDot.Click += GameCard_Click;

            gameCardPanel.Controls.AddRange(new Control[] { gameIcon, gameTitle, gameStatus, statusDot });
            mainContainer.Controls.Add(gameCardPanel);
        }

        private void SetupActionPanel()
        {
            actionPanel = new Panel()
            {
                Size = new Size(420, 90),
                Location = new Point(30, 230),
                BackColor = Color.Transparent
            };

            injectButton = new Button()
            {
                Text = "INJECT",
                Size = new Size(140, 45),
                Location = new Point(140, 22),
                BackColor = Color.Transparent,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                Cursor = Cursors.Hand,
                Visible = false
            };
            injectButton.FlatAppearance.BorderSize = 0;
            injectButton.Paint += InjectButton_Paint;
            injectButton.Click += InjectButton_Click;

            hoverAnimations[injectButton] = 0f;
            isHovered[injectButton] = false;
            injectButton.MouseEnter += (s, e) => { isHovered[injectButton] = true; };
            injectButton.MouseLeave += (s, e) => { isHovered[injectButton] = false; };

            // Premium progress container
            progressContainer = new Panel()
            {
                Size = new Size(380, 35),
                Location = new Point(20, 27),
                BackColor = Color.Transparent,
                Visible = false
            };

            progressBar = new Panel()
            {
                Size = new Size(380, 6),
                Location = new Point(0, 0),
                BackColor = Color.Transparent
            };
            progressBar.Paint += ProgressBar_Paint;

            progressText = new Label()
            {
                Text = "Initializing...",
                Font = new Font("Segoe UI", 10F),
                ForeColor = Color.FromArgb(160, 170, 180),
                BackColor = Color.Transparent,
                Location = new Point(0, 12),
                Size = new Size(380, 23),
                TextAlign = ContentAlignment.MiddleCenter
            };

            progressContainer.Controls.AddRange(new Control[] { progressBar, progressText });
            actionPanel.Controls.AddRange(new Control[] { injectButton, progressContainer });
            mainContainer.Controls.Add(actionPanel);
        }

        private void SetupAnimationTracking()
        {
            // Initialize hover animations for all tracked controls
            foreach (var control in hoverAnimations.Keys)
            {
                hoverAnimations[control] = 0f;
                isHovered[control] = false;
            }
        }

        private void StartAdvancedAnimations()
        {
            animationTimer = new Timer();
            animationTimer.Interval = 16; // 60 FPS
            animationTimer.Tick += (s, e) =>
            {
                pulseAnimation += 0.08f;
                glowAnimation += 0.05f;
                backgroundParticles += 0.02f;

                if (pulseAnimation > Math.PI * 2) pulseAnimation = 0f;
                if (glowAnimation > Math.PI * 2) glowAnimation = 0f;
                if (backgroundParticles > Math.PI * 2) backgroundParticles = 0f;

                // Update hover animations with smooth easing
                foreach (var kvp in hoverAnimations.ToList())
                {
                    Control control = kvp.Key;
                    float target = isHovered.ContainsKey(control) && isHovered[control] ? 1f : 0f;
                    float current = kvp.Value;
                    float newValue = current + (target - current) * 0.15f; // Smooth easing
                    hoverAnimations[control] = Math.Abs(newValue - target) < 0.01f ? target : newValue;
                }

                // Invalidate animated controls
                statusDot?.Invalidate();
                backgroundEffect?.Invalidate();
                gameCardPanel?.Invalidate();
                injectButton?.Invalidate();
                minimizeButton?.Invalidate();
                closeButton?.Invalidate();
            };
            animationTimer.Start();
        }

        private void BackgroundEffect_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            // Animated gradient background
            Rectangle bgRect = new Rectangle(0, 0, this.Width, this.Height);

            float animOffset = (float)(Math.Sin(backgroundParticles) * 0.1f);

            using (LinearGradientBrush bgBrush = new LinearGradientBrush(
                bgRect,
                Color.FromArgb(10, 10, 15),
                Color.FromArgb((int)(15 + animOffset * 10), (int)(15 + animOffset * 8), (int)(25 + animOffset * 5)),
                LinearGradientMode.ForwardDiagonal))
            {
                g.FillRectangle(bgBrush, bgRect);
            }

            // Subtle floating particles effect
            Random rand = new Random(12345); // Fixed seed for consistent particles
            for (int i = 0; i < 8; i++)
            {
                float x = rand.Next(0, this.Width);
                float y = rand.Next(0, this.Height);
                float particleOffset = (float)(Math.Sin(backgroundParticles + i) * 2);

                using (SolidBrush particleBrush = new SolidBrush(Color.FromArgb(15, 100, 150, 255)))
                {
                    g.FillEllipse(particleBrush, x + particleOffset, y + particleOffset, 3, 3);
                }
            }
        }

        private void MainContainer_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            Rectangle rect = new Rectangle(0, 0, mainContainer.Width, mainContainer.Height);

            // Advanced glassmorphism effect
            using (GraphicsPath path = CreateRoundedRectangle(rect, 24))
            {
                // Multiple layer backdrop blur simulation
                using (LinearGradientBrush layer1 = new LinearGradientBrush(
                    rect,
                    Color.FromArgb(45, 255, 255, 255),
                    Color.FromArgb(15, 255, 255, 255),
                    LinearGradientMode.Vertical))
                {
                    g.FillPath(layer1, path);
                }

                using (LinearGradientBrush layer2 = new LinearGradientBrush(
                    rect,
                    Color.FromArgb(25, 100, 150, 255),
                    Color.FromArgb(8, 100, 150, 255),
                    LinearGradientMode.BackwardDiagonal))
                {
                    g.FillPath(layer2, path);
                }

                // Premium border with gradient
                using (LinearGradientBrush borderBrush = new LinearGradientBrush(
                    rect,
                    Color.FromArgb(120, 255, 255, 255),
                    Color.FromArgb(40, 255, 255, 255),
                    LinearGradientMode.Vertical))
                using (Pen borderPen = new Pen(borderBrush, 1.5f))
                {
                    g.DrawPath(borderPen, path);
                }

                // Subtle inner glow
                Rectangle innerRect = new Rectangle(rect.X + 1, rect.Y + 1, rect.Width - 2, rect.Height - 2);
                using (GraphicsPath innerPath = CreateRoundedRectangle(innerRect, 23))
                using (Pen innerPen = new Pen(Color.FromArgb(30, 255, 255, 255), 1))
                {
                    g.DrawPath(innerPen, innerPath);
                }
            }
        }

        private void HeaderPanel_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            // Elegant header separator with gradient
            using (LinearGradientBrush separatorBrush = new LinearGradientBrush(
                new Point(30, headerPanel.Height - 2),
                new Point(headerPanel.Width - 30, headerPanel.Height - 2),
                Color.FromArgb(50, 255, 255, 255),
                Color.FromArgb(10, 255, 255, 255)))
            using (Pen separatorPen = new Pen(separatorBrush, 1))
            {
                g.DrawLine(separatorPen, 30, headerPanel.Height - 2, headerPanel.Width - 30, headerPanel.Height - 2);
            }
        }

        private void WindowButton_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            Button btn = sender as Button;
            Rectangle rect = new Rectangle(0, 0, btn.Width, btn.Height);
            float hoverLevel = hoverAnimations[btn];
            bool isCloseBtn = btn.Tag?.ToString() == "close";

            // Smooth hover background
            if (hoverLevel > 0)
            {
                Color hoverColor = isCloseBtn ?
                    Color.FromArgb((int)(120 * hoverLevel), 220, 50, 47) :
                    Color.FromArgb((int)(40 * hoverLevel), 255, 255, 255);

                using (SolidBrush hoverBrush = new SolidBrush(hoverColor))
                using (GraphicsPath path = CreateRoundedRectangle(rect, 6))
                {
                    g.FillPath(hoverBrush, path);
                }
            }

            // Button text with smooth color transition
            Color textColor = isCloseBtn && hoverLevel > 0.5f ?
                Color.White :
                Color.FromArgb(180 + (int)(75 * hoverLevel), 180 + (int)(75 * hoverLevel), 180 + (int)(75 * hoverLevel));

            using (SolidBrush textBrush = new SolidBrush(textColor))
            {
                StringFormat sf = new StringFormat()
                {
                    Alignment = StringAlignment.Center,
                    LineAlignment = StringAlignment.Center
                };
                g.DrawString(btn.Text, btn.Font, textBrush, btn.ClientRectangle, sf);
            }
        }

        private void GameCardPanel_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            Rectangle rect = new Rectangle(0, 0, gameCardPanel.Width, gameCardPanel.Height);
            float hoverLevel = hoverAnimations[gameCardPanel];

            using (GraphicsPath path = CreateRoundedRectangle(rect, 18))
            {
                // Base glass background
                Color baseColor = Color.FromArgb(20 + (int)(15 * hoverLevel), 255, 255, 255);
                using (SolidBrush baseBrush = new SolidBrush(baseColor))
                {
                    g.FillPath(baseBrush, path);
                }

                // Premium border with enhanced hover effect
                Color borderColor = Color.FromArgb(40 + (int)(40 * hoverLevel), 100, 150, 255);
                using (Pen borderPen = new Pen(borderColor, 1 + hoverLevel))
                {
                    g.DrawPath(borderPen, path);
                }

                // Glow effect on hover
                if (hoverLevel > 0)
                {
                    for (int i = 1; i <= 6; i++)
                    {
                        Rectangle glowRect = new Rectangle(rect.X - i, rect.Y - i,
                                                         rect.Width + i * 2, rect.Height + i * 2);
                        using (GraphicsPath glowPath = CreateRoundedRectangle(glowRect, 18 + i))
                        using (Pen glowPen = new Pen(Color.FromArgb((int)(15 * hoverLevel), 100, 150, 255), 1))
                        {
                            g.DrawPath(glowPen, glowPath);
                        }
                    }
                }
            }
        }

        private void GameIcon_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            Rectangle iconRect = new Rectangle(8, 8, 54, 54);

            // Premium gradient background
            using (LinearGradientBrush iconBrush = new LinearGradientBrush(
                iconRect,
                Color.FromArgb(255, 88, 130, 235),
                Color.FromArgb(255, 67, 97, 238),
                LinearGradientMode.ForwardDiagonal))
            {
                g.FillEllipse(iconBrush, iconRect);
            }

            // Outer glow
            for (int i = 1; i <= 3; i++)
            {
                Rectangle glowRect = new Rectangle(iconRect.X - i, iconRect.Y - i,
                                                 iconRect.Width + i * 2, iconRect.Height + i * 2);
                using (Pen glowPen = new Pen(Color.FromArgb(20, 88, 130, 235), 1))
                {
                    g.DrawEllipse(glowPen, glowRect);
                }
            }

            // Premium border
            using (Pen iconPen = new Pen(Color.FromArgb(150, 255, 255, 255), 2))
            {
                g.DrawEllipse(iconPen, iconRect);
            }

            // CS2 text with shadow
            using (SolidBrush shadowBrush = new SolidBrush(Color.FromArgb(100, 0, 0, 0)))
            using (SolidBrush textBrush = new SolidBrush(Color.White))
            {
                Font iconFont = new Font("Segoe UI", 13F, FontStyle.Bold);
                StringFormat sf = new StringFormat()
                {
                    Alignment = StringAlignment.Center,
                    LineAlignment = StringAlignment.Center
                };

                Rectangle shadowRect = new Rectangle(iconRect.X + 1, iconRect.Y + 1, iconRect.Width, iconRect.Height);
                g.DrawString("CS2", iconFont, shadowBrush, shadowRect, sf);
                g.DrawString("CS2", iconFont, textBrush, iconRect, sf);
            }
        }

        private void StatusDot_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            Color dotColor = Color.FromArgb(255, 156, 163, 175);
            if (isGameDetected && !isInjected)
                dotColor = Color.FromArgb(255, 34, 197, 94);
            else if (isInjected)
                dotColor = Color.FromArgb(255, 59, 130, 246);
            else if (!isGameDetected)
                dotColor = Color.FromArgb(255, 239, 68, 68);

            // Premium animated pulse
            float pulseScale = 1.0f + (float)(Math.Sin(pulseAnimation) * 0.25f);
            int size = (int)(10 * pulseScale);
            int offset = (16 - size) / 2;

            // Multi-layer glow effect
            for (int i = 1; i <= 5; i++)
            {
                int glowSize = size + i * 3;
                int glowOffset = (16 - glowSize) / 2;
                int alpha = Math.Max(5, 30 - i * 5);
                using (SolidBrush glowBrush = new SolidBrush(Color.FromArgb(alpha, dotColor)))
                {
                    g.FillEllipse(glowBrush, glowOffset, glowOffset, glowSize, glowSize);
                }
            }

            // Main dot with gradient
            Rectangle dotRect = new Rectangle(offset, offset, size, size);
            using (LinearGradientBrush dotBrush = new LinearGradientBrush(
                dotRect, dotColor, ColorBrightness(dotColor, 0.8f), LinearGradientMode.Vertical))
            {
                g.FillEllipse(dotBrush, dotRect);
            }

            // Highlight
            int highlightSize = Math.Max(2, size / 3);
            using (SolidBrush highlightBrush = new SolidBrush(Color.FromArgb(150, 255, 255, 255)))
            {
                g.FillEllipse(highlightBrush, offset + 2, offset + 2, highlightSize, highlightSize);
            }
        }

        private void InjectButton_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            Rectangle rect = new Rectangle(0, 0, injectButton.Width, injectButton.Height);
            float hoverLevel = hoverAnimations[injectButton];

            using (GraphicsPath path = CreateRoundedRectangle(rect, 12))
            {
                // Premium button gradient
                Color startColor = isInjected ?
                    Color.FromArgb(34, 197, 94) :
                    Color.FromArgb(59, 130, 246);
                Color endColor = isInjected ?
                    Color.FromArgb(21, 128, 61) :
                    Color.FromArgb(37, 99, 235);

                if (hoverLevel > 0 && !isInjecting)
                {
                    startColor = ColorBrightness(startColor, 1.0f + 0.2f * hoverLevel);
                    endColor = ColorBrightness(endColor, 1.0f + 0.2f * hoverLevel);
                }

                using (LinearGradientBrush buttonBrush = new LinearGradientBrush(
                    rect, startColor, endColor, LinearGradientMode.Vertical))
                {
                    g.FillPath(buttonBrush, path);
                }

                // Enhanced border
                using (Pen borderPen = new Pen(Color.FromArgb(120 + (int)(60 * hoverLevel), 255, 255, 255), 1))
                {
                    g.DrawPath(borderPen, path);
                }

                // Glow effect on hover
                if (hoverLevel > 0)
                {
                    for (int i = 1; i <= 4; i++)
                    {
                        Rectangle glowRect = new Rectangle(rect.X - i, rect.Y - i,
                                                         rect.Width + i * 2, rect.Height + i * 2);
                        using (GraphicsPath glowPath = CreateRoundedRectangle(glowRect, 12 + i))
                        using (Pen glowPen = new Pen(Color.FromArgb((int)(20 * hoverLevel), startColor), 1))
                        {
                            g.DrawPath(glowPen, glowPath);
                        }
                    }
                }
            }

            // Button text with shadow
            string buttonText = isInjected ? "✓ INJECTED" : (isInjecting ? "INJECTING..." : "INJECT");
            using (SolidBrush shadowBrush = new SolidBrush(Color.FromArgb(100, 0, 0, 0)))
            using (SolidBrush textBrush = new SolidBrush(Color.White))
            {
                StringFormat sf = new StringFormat()
                {
                    Alignment = StringAlignment.Center,
                    LineAlignment = StringAlignment.Center
                };

                Rectangle shadowRect = new Rectangle(1, 1, rect.Width, rect.Height);
                g.DrawString(buttonText, injectButton.Font, shadowBrush, shadowRect, sf);
                g.DrawString(buttonText, injectButton.Font, textBrush, rect, sf);
            }
        }

        private void ProgressBar_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            Rectangle bgRect = new Rectangle(0, 0, progressBar.Width, progressBar.Height);

            // Premium background
            using (GraphicsPath bgPath = CreateRoundedRectangle(bgRect, 3))
            using (LinearGradientBrush bgBrush = new LinearGradientBrush(
                bgRect,
                Color.FromArgb(40, 255, 255, 255),
                Color.FromArgb(20, 255, 255, 255),
                LinearGradientMode.Vertical))
            {
                g.FillPath(bgBrush, bgPath);
            }

            // Animated progress fill
            if (progressValue > 0)
            {
                int progressWidth = (int)(progressBar.Width * (progressValue / 100f));
                Rectangle progressRect = new Rectangle(0, 0, progressWidth, progressBar.Height);

                using (GraphicsPath progressPath = CreateRoundedRectangle(progressRect, 3))
                {
                    // Gradient progress fill
                    using (LinearGradientBrush progressBrush = new LinearGradientBrush(
                        progressRect,
                        Color.FromArgb(59, 130, 246),
                        Color.FromArgb(37, 99, 235),
                        LinearGradientMode.Horizontal))
                    {
                        g.FillPath(progressBrush, progressPath);
                    }

                    // Animated shine effect
                    float shineOffset = (float)(Math.Sin(glowAnimation) * 0.3f + 0.5f);
                    if (progressWidth > 20)
                    {
                        Rectangle shineRect = new Rectangle((int)(progressWidth * shineOffset) - 10, 0, 20, progressBar.Height);
                        using (LinearGradientBrush shineBrush = new LinearGradientBrush(
                            shineRect,
                            Color.FromArgb(80, 255, 255, 255),
                            Color.FromArgb(0, 255, 255, 255),
                            LinearGradientMode.Horizontal))
                        {
                            g.FillRectangle(shineBrush, shineRect);
                        }
                    }
                }
            }
        }

        private GraphicsPath CreateRoundedRectangle(Rectangle rect, int radius)
        {
            GraphicsPath path = new GraphicsPath();
            int diameter = radius * 2;

            path.AddArc(rect.X, rect.Y, diameter, diameter, 180, 90);
            path.AddArc(rect.Right - diameter, rect.Y, diameter, diameter, 270, 90);
            path.AddArc(rect.Right - diameter, rect.Bottom - diameter, diameter, diameter, 0, 90);
            path.AddArc(rect.X, rect.Bottom - diameter, diameter, diameter, 90, 90);
            path.CloseFigure();

            return path;
        }

        private Color ColorBrightness(Color color, float factor)
        {
            return Color.FromArgb(
                color.A,
                Math.Min(255, (int)(color.R * factor)),
                Math.Min(255, (int)(color.G * factor)),
                Math.Min(255, (int)(color.B * factor))
            );
        }

        private void GameCard_Click(object sender, EventArgs e)
        {
            if (isGameDetected && !isInjected && !isInjecting)
            {
                injectButton.Visible = true;
                progressContainer.Visible = false;
            }
        }

        private async void InjectButton_Click(object sender, EventArgs e)
        {
            if (isInjected || isInjecting) return;
            await PerformInjection();
        }

        private async Task PerformInjection()
        {
            isInjecting = true;
            injectButton.Visible = false;
            progressContainer.Visible = true;
            progressValue = 0;

            string[] steps = {
                "Initializing injection...",
                "Bypassing security hooks...",
                "Allocating memory space...",
                "Injecting payload...",
                "Finalizing process..."
            };

            try
            {
                for (int i = 0; i < steps.Length; i++)
                {
                    progressText.Text = steps[i];

                    for (int j = 0; j < 20; j++)
                    {
                        progressValue = (i * 20) + j;
                        progressBar.Invalidate();
                        await Task.Delay(75);
                    }
                }

                progressValue = 100;
                progressBar.Invalidate();
                await Task.Delay(800);

                // Simulate injection success
                bool success = true; // Replace with actual injection logic

                if (success)
                {
                    isInjected = true;
                    gameStatus.Text = "Successfully injected! ✓";
                    gameStatus.ForeColor = Color.FromArgb(34, 197, 94);
                    injectButton.Visible = true;
                    injectButton.Invalidate();
                }
                else
                {
                    throw new Exception("Injection failed");
                }
            }
            catch (Exception ex)
            {
                gameStatus.Text = $"Error: {ex.Message}";
                gameStatus.ForeColor = Color.FromArgb(239, 68, 68);
                injectButton.Visible = true;
            }
            finally
            {
                isInjecting = false;
                progressContainer.Visible = false;
            }
        }

        private async void CheckGameStatus()
        {
            await Task.Delay(1500);

            var processes = System.Diagnostics.Process.GetProcessesByName("cs2");

            if (processes.Length > 0)
            {
                isGameDetected = true;
                gameStatus.Text = "Game detected • Click to inject";
                gameStatus.ForeColor = Color.FromArgb(34, 197, 94);
            }
            else
            {
                gameStatus.Text = "Game not found • Launch CS2 first";
                gameStatus.ForeColor = Color.FromArgb(239, 68, 68);
            }
        }

        private void Form_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                NativeMethods.ReleaseCapture();
                NativeMethods.SendMessage(this.Handle, NativeMethods.WM_NCLBUTTONDOWN, NativeMethods.HT_CAPTION, 0);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                animationTimer?.Stop();
                animationTimer?.Dispose();
            }
            base.Dispose(disposing);
        }
    }

    internal static class NativeMethods
    {
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern bool ReleaseCapture();
    }
}
