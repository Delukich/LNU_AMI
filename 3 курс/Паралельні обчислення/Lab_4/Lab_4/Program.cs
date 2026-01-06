using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using System.Collections.Generic;

namespace ThreadVisualization
{
    public class MainForm : Form
    {
        private Panel cylinder1, cylinder2, cylinder3;
        private Label label1, label2, label3, statusLabel;
        private Button btnStart, btnReset, btnRandom;
        private TextBox txtT11, txtT21, txtT31;
        private List<CylinderSegment> segments1, segments2, segments3;
        private bool isRunning = false;

        private int t11 = 3000, t21 = 2000, t31 = 4000;
        private System.Windows.Forms.Timer updateTimer;

        private int? nextTimeForThread2 = null;
        private int? nextTimeForThread3 = null;
        private int? nextTimeForThread1 = null;

        private CylinderSegment currentSegment1, currentSegment2, currentSegment3;
        private DateTime segmentStart1, segmentStart2, segmentStart3;

        private Thread thread1, thread2, thread3;

        public MainForm()
        {
            InitializeComponents();
            segments1 = new List<CylinderSegment>();
            segments2 = new List<CylinderSegment>();
            segments3 = new List<CylinderSegment>();

            updateTimer = new System.Windows.Forms.Timer();
            updateTimer.Interval = 50;
            updateTimer.Tick += UpdateTimer_Tick;
        }

        private void InitializeComponents()
        {
            this.Text = "Візуалізація роботи 3-х потоків";
            this.Size = new Size(950, 650);
            this.StartPosition = FormStartPosition.CenterScreen;

            Panel controlPanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 110,
                BackColor = Color.LightGray
            };

            Label lblT1 = new Label { Text = "T1,1 (мс):", Location = new Point(10, 15), Width = 70 };
            txtT11 = new TextBox { Location = new Point(85, 12), Width = 60, Text = "3000" };

            Label lblT2 = new Label { Text = "T2,1 (мс):", Location = new Point(160, 15), Width = 70 };
            txtT21 = new TextBox { Location = new Point(235, 12), Width = 60, Text = "2000" };

            Label lblT3 = new Label { Text = "T3,1 (мс):", Location = new Point(310, 15), Width = 70 };
            txtT31 = new TextBox { Location = new Point(385, 12), Width = 60, Text = "4000" };

            btnStart = new Button { Text = "Старт", Location = new Point(460, 10), Width = 80, Height = 30 };
            btnStart.Click += BtnStart_Click;

            btnReset = new Button { Text = "Скинути", Location = new Point(550, 10), Width = 80, Height = 30 };
            btnReset.Click += BtnReset_Click;

            btnRandom = new Button { Text = "Рандом", Location = new Point(640, 10), Width = 80, Height = 30 };
            btnRandom.Click += BtnRandom_Click;

            Label lblInfo = new Label
            {
                Text = "Синій - робота, Червоний - простій (очікування)",
                Location = new Point(10, 50),
                AutoSize = true
            };

            statusLabel = new Label
            {
                Text = "Готово до запуску",
                Location = new Point(10, 75),
                Width = 700,
                Font = new Font("Arial", 9, FontStyle.Bold),
                ForeColor = Color.DarkGreen
            };

            controlPanel.Controls.AddRange(new Control[]
            {
                lblT1, txtT11, lblT2, txtT21, lblT3, txtT31,
                btnStart, btnReset, btnRandom, lblInfo, statusLabel
            });

            Panel cylindersPanel = new Panel { Dock = DockStyle.Fill, BackColor = Color.White };

            label1 = new Label { Text = "Потік 1", Location = new Point(80, 20), Width = 100, Font = new Font("Arial", 11, FontStyle.Bold) };
            cylinder1 = new Panel { Location = new Point(50, 50), Size = new Size(80, 450), BorderStyle = BorderStyle.FixedSingle, BackColor = Color.White };
            cylinder1.Paint += Cylinder1_Paint;

            label2 = new Label { Text = "Потік 2", Location = new Point(230, 20), Width = 100, Font = new Font("Arial", 11, FontStyle.Bold) };
            cylinder2 = new Panel { Location = new Point(200, 50), Size = new Size(80, 450), BorderStyle = BorderStyle.FixedSingle, BackColor = Color.White };
            cylinder2.Paint += Cylinder2_Paint;

            label3 = new Label { Text = "Потік 3", Location = new Point(380, 20), Width = 100, Font = new Font("Arial", 11, FontStyle.Bold) };
            cylinder3 = new Panel { Location = new Point(350, 50), Size = new Size(80, 450), BorderStyle = BorderStyle.FixedSingle, BackColor = Color.White };
            cylinder3.Paint += Cylinder3_Paint;

            cylindersPanel.Controls.AddRange(new Control[] { label1, cylinder1, label2, cylinder2, label3, cylinder3 });

            this.Controls.Add(cylindersPanel);
            this.Controls.Add(controlPanel);
        }

        private void UpdateTimer_Tick(object sender, EventArgs e)
        {
            UpdateCurrentSegment(1);
            UpdateCurrentSegment(2);
            UpdateCurrentSegment(3);

            cylinder1.Invalidate();
            cylinder2.Invalidate();
            cylinder3.Invalidate();
        }

        private void UpdateCurrentSegment(int threadNum)
        {
            CylinderSegment current = threadNum == 1 ? currentSegment1 :
                                     threadNum == 2 ? currentSegment2 : currentSegment3;
            DateTime startTime = threadNum == 1 ? segmentStart1 :
                                threadNum == 2 ? segmentStart2 : segmentStart3;

            if (current != null)
            {
                int elapsed = (int)(DateTime.Now - startTime).TotalMilliseconds;
                current.CurrentDuration = Math.Min(elapsed, current.Duration);
            }
        }

        private void BtnStart_Click(object sender, EventArgs e)
        {
            if (isRunning) return;

            if (!int.TryParse(txtT11.Text, out t11) ||
                !int.TryParse(txtT21.Text, out t21) ||
                !int.TryParse(txtT31.Text, out t31))
            {
                MessageBox.Show("Введіть коректні числові значення!");
                return;
            }

            isRunning = true;
            btnStart.Enabled = false;

            segments1.Clear();
            segments2.Clear();
            segments3.Clear();

            currentSegment1 = null;
            currentSegment2 = null;
            currentSegment3 = null;

            nextTimeForThread1 = null;
            nextTimeForThread2 = null;
            nextTimeForThread3 = null;

            updateTimer.Start();

            thread1 = new Thread(Thread1Work) { IsBackground = true };
            thread2 = new Thread(Thread2Work) { IsBackground = true };
            thread3 = new Thread(Thread3Work) { IsBackground = true };

            thread1.Start();
            thread2.Start();
            thread3.Start();

            new Thread(() =>
            {
                thread1.Join();
                thread2.Join();
                thread3.Join();

                this.Invoke((MethodInvoker)(() =>
                {
                    updateTimer.Stop();
                    UpdateStatus("Завершено!");
                    btnStart.Enabled = true;
                    isRunning = false;
                }));
            }).Start();
        }

        private void BtnReset_Click(object sender, EventArgs e)
        {
            segments1.Clear();
            segments2.Clear();
            segments3.Clear();
            currentSegment1 = null;
            currentSegment2 = null;
            currentSegment3 = null;
            cylinder1.Invalidate();
            cylinder2.Invalidate();
            cylinder3.Invalidate();
            UpdateStatus("Очищено");
        }

        private void BtnRandom_Click(object sender, EventArgs e)
        {
            Random rand = new Random();
            txtT11.Text = (rand.Next(2, 6) * 1000).ToString();
            txtT21.Text = (rand.Next(2, 6) * 1000).ToString();
            txtT31.Text = (rand.Next(2, 6) * 1000).ToString();
        }

        private void UpdateStatus(string message)
        {
            if (statusLabel.InvokeRequired)
                statusLabel.Invoke((MethodInvoker)delegate { statusLabel.Text = message; });
            else
                statusLabel.Text = message;
        }

        private void Thread1Work()
        {
            int cycles = 4;
            int currentTime = t11;
            Random rand = new Random(1);

            for (int i = 0; i < cycles; i++)
            {
                if (i > 0)
                {
                    DateTime startWait = DateTime.Now;
                    while (nextTimeForThread1 == null)
                    {
                        Thread.Sleep(50);
                        int waitTime = (int)(DateTime.Now - startWait).TotalMilliseconds;
                        if (waitTime > 50)
                        {
                            AddSegment(1, 50, Color.Red, "Очікування");
                            startWait = DateTime.Now;
                        }
                    }
                    currentTime = nextTimeForThread1.Value;
                    nextTimeForThread1 = null;
                }

                UpdateStatus($"Потік 1 працює {currentTime}мс");
                AddSegment(1, currentTime, Color.Blue, $"{currentTime}мс");

                nextTimeForThread2 = rand.Next(1500, 5000);
            }
        }

        private void Thread2Work()
        {
            int cycles = 4;
            int currentTime = t21;
            Random rand = new Random(2);

            for (int i = 0; i < cycles; i++)
            {
                if (i > 0)
                {
                    DateTime startWait = DateTime.Now;
                    while (nextTimeForThread2 == null)
                    {
                        Thread.Sleep(50);
                        int waitTime = (int)(DateTime.Now - startWait).TotalMilliseconds;
                        if (waitTime > 50)
                        {
                            AddSegment(2, 50, Color.Red, "Очікування");
                            startWait = DateTime.Now;
                        }
                    }
                    currentTime = nextTimeForThread2.Value;
                    nextTimeForThread2 = null;
                }

                UpdateStatus($"Потік 2 працює {currentTime}мс");
                AddSegment(2, currentTime, Color.Blue, $"{currentTime}мс");

                nextTimeForThread3 = rand.Next(1500, 5000);
            }
        }

        private void Thread3Work()
        {
            int cycles = 4;
            int currentTime = t31;
            Random rand = new Random(3);

            for (int i = 0; i < cycles; i++)
            {
                if (i > 0)
                {
                    DateTime startWait = DateTime.Now;
                    while (nextTimeForThread3 == null)
                    {
                        Thread.Sleep(50);
                        int waitTime = (int)(DateTime.Now - startWait).TotalMilliseconds;
                        if (waitTime > 50)
                        {
                            AddSegment(3, 50, Color.Red, "Очікування");
                            startWait = DateTime.Now;
                        }
                    }
                    currentTime = nextTimeForThread3.Value;
                    nextTimeForThread3 = null;
                }

                UpdateStatus($"Потік 3 працює {currentTime}мс");
                AddSegment(3, currentTime, Color.Blue, $"{currentTime}мс");

                nextTimeForThread1 = rand.Next(1500, 5000);
            }
        }

        private void AddSegment(int threadNum, int duration, Color color, string label)
        {
            CylinderSegment segment = new CylinderSegment
            {
                Duration = duration,
                CurrentDuration = 0,
                Color = color,
                Label = label
            };

            if (threadNum == 1) { currentSegment1 = segment; segmentStart1 = DateTime.Now; }
            else if (threadNum == 2) { currentSegment2 = segment; segmentStart2 = DateTime.Now; }
            else { currentSegment3 = segment; segmentStart3 = DateTime.Now; }

            int elapsed = 0;
            while (elapsed < duration)
            {
                Thread.Sleep(50);
                elapsed += 50;
                int finalElapsed = elapsed;
                if (threadNum == 1) currentSegment1.CurrentDuration = Math.Min(finalElapsed, duration);
                else if (threadNum == 2) currentSegment2.CurrentDuration = Math.Min(finalElapsed, duration);
                else currentSegment3.CurrentDuration = Math.Min(finalElapsed, duration);
            }

            List<CylinderSegment> segments = threadNum == 1 ? segments1 :
                                            threadNum == 2 ? segments2 : segments3;

            this.Invoke((MethodInvoker)(() =>
            {
                segments.Add(segment);
                if (threadNum == 1) currentSegment1 = null;
                else if (threadNum == 2) currentSegment2 = null;
                else currentSegment3 = null;
            }));
        }

        private void DrawCylinder(Panel cylinder, List<CylinderSegment> segments,
                                 CylinderSegment currentSegment, PaintEventArgs e)
        {
            int totalDuration = 0;
            foreach (var seg in segments) totalDuration += seg.Duration;
            if (currentSegment != null) totalDuration += currentSegment.Duration;
            if (totalDuration == 0) return;

            float currentY = 0;
            float heightPerMs = cylinder.Height / (float)totalDuration;

            foreach (var seg in segments)
            {
                float segmentHeight = seg.Duration * heightPerMs;
                DrawSegment(e.Graphics, seg, 0, currentY, cylinder.Width, segmentHeight);
                currentY += segmentHeight;
            }

            if (currentSegment != null)
            {
                float totalSegmentHeight = currentSegment.Duration * heightPerMs;
                float currentHeight = currentSegment.CurrentDuration * heightPerMs;

                if (currentHeight > 0) DrawSegment(e.Graphics, currentSegment, 0, currentY, cylinder.Width, currentHeight);

                if (totalSegmentHeight > currentHeight)
                {
                    using (Pen dashedPen = new Pen(Color.Gray, 2))
                    {
                        dashedPen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
                        e.Graphics.DrawRectangle(dashedPen, 0, currentY, cylinder.Width - 1, totalSegmentHeight);
                    }
                }
            }
        }

        private void DrawSegment(Graphics g, CylinderSegment seg, float x, float y, float width, float height)
        {
            using (SolidBrush brush = new SolidBrush(seg.Color))
            {
                g.FillRectangle(brush, x, y, width, height);
            }

            g.DrawRectangle(Pens.Black, x, y, width - 1, height);

            if (height > 20)
            {
                using (Font font = new Font("Arial", 7, FontStyle.Bold))
                using (SolidBrush textBrush = new SolidBrush(Color.White))
                {
                    StringFormat sf = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
                    RectangleF rect = new RectangleF(x, y, width, height);
                    g.DrawString(seg.Label, font, textBrush, rect, sf);
                }
            }
        }

        private void Cylinder1_Paint(object sender, PaintEventArgs e) { DrawCylinder(cylinder1, segments1, currentSegment1, e); }
        private void Cylinder2_Paint(object sender, PaintEventArgs e) { DrawCylinder(cylinder2, segments2, currentSegment2, e); }
        private void Cylinder3_Paint(object sender, PaintEventArgs e) { DrawCylinder(cylinder3, segments3, currentSegment3, e); }
    }

    public class CylinderSegment
    {
        public int Duration { get; set; }
        public int CurrentDuration { get; set; }
        public Color Color { get; set; }
        public string Label { get; set; }
    }

    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }
}
