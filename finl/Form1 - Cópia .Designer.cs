using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Policy;
using System.Windows.Forms;

namespace finl
{
    partial class Form1
    {
        /// <summary>
        /// Variável de designer necessária.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpar os recursos que estão sendo usados.
        /// </summary>
        /// <param name="disposing">true se for necessário descartar os recursos gerenciados; caso contrário, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }


        /// <summary>
        /// Método necessário para suporte ao Designer - não modifique 
        /// o conteúdo deste método com o editor de código.
        /// </summary>
        /// 
        /// // Source - https://stackoverflow.com/a/61719
        // Posted by Brad Bruce
        // Retrieved 2026-06-16, License - CC BY-SA 2.5
        /*
                public class RefreshingListBox : ListBox
                {
                    public new void RefreshItem(int index)
                    {
                        base.RefreshItem(index);
                    }

                    public new void RefreshItems()
                    {
                        base.RefreshItems();
                    }
                }*/
        public BindingList<string> boundlist;
        public List<string> forbox;
        public Timer clicktimer;
        public Rectangle dragBoxFromMouseDown;
        public List<string> staticspri = new List<string>() { @"C:\Users\enzo\Desktop\finl\finl\ame sprites\stream_ame_comic_000.png", @"C:\Users\enzo\Desktop\finl\finl\ame sprites\stream_ame_craziness_000.png" };


        private void InitializeComponent()
        {


            this.components = new System.ComponentModel.Container();
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(680, 450);
            this.Text = "Main panel";

            frm1 = this;

            forbox = new List<string>();

            this.bg = new System.Windows.Forms.PictureBox();
            this.bg.Size = new System.Drawing.Size(348, 227);
            this.bg.Location = new System.Drawing.Point(50, 50);
            this.bg.Name = "bg";
            this.bg.Image = Image.FromFile(@"C:\Users\enzo\Desktop\finl\finl\backgrounds\bg_stream.png");
            bg.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.Controls.Add(bg);

            this.sprp = new System.Windows.Forms.PictureBox();
            this.sprp.Size = new System.Drawing.Size(348, 227);
            this.sprp.Name = "bg";
            sprp.Parent = bg;
            sprp.BackColor = Color.Transparent;
            this.sprp.Image = Image.FromFile(@"C:\Users\enzo\Desktop\finl\finl\ame sprites\stream_ame_comic_000.png");
            sprp.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            bg.Controls.Add(sprp);

            jk = new Label();
            jk.Location = new Point(50, 300);
            jk.Size = new Size(500, 30);
            jk.Text = "Stop!";
            this.Controls.Add(jk);

            boundlist = new BindingList<string>(forbox);


            lol = new ListBox();
            lol.Location = new Point(400, 50);
            lol.Size = new System.Drawing.Size(270, 227);
            lol.DataSource = boundlist;
            lol.Click += (sender, e) =>
            {
                int i = lol.SelectedIndex;
                if (i != -1)
                {
                    jk.Text = Convert.ToString(i);
                    bg.Image = Image.FromFile(@"C:\Users\enzo\Desktop\finl\finl\backgrounds\" + forbox[i].Replace(" ", "_") + ".png");
                    boundlist.ResetBindings();
                    lol.SelectedIndex = i;
                    boundlist.ResetBindings();

                }
            };
            lol.DoubleClick += (sender, e) =>
            {
                int i = lol.SelectedIndex;
                forbox.RemoveAt(i);
                boundlist.ResetBindings();

            };

            lol.AllowDrop = true;


            lol.MouseMove += lol_MouseMove;
            lol.MouseDown += lol_MouseDown;

            lol.DragOver += lol_DragOver;
            lol.DragDrop += lol_DragDrop;

            this.Controls.Add(lol);
        }
        // Triggered when the user clicks down on an item to begin dragging
        private int clickedIndex = ListBox.NoMatches;

        private void lol_MouseDown(object sender, MouseEventArgs e)
        {
            // 1. Immediately find what was clicked
            clickedIndex = lol.IndexFromPoint(e.Location);

            if (clickedIndex != ListBox.NoMatches)
            {
                // 2. FORCE the ListBox to select the item right now
                lol.SelectedIndex = clickedIndex;

                // 3. Create the drag threshold boundary box
                Size dragSize = SystemInformation.DragSize;
                dragBoxFromMouseDown = new Rectangle(
                    new Point(e.X - (dragSize.Width / 2), e.Y - (dragSize.Height / 2)),
                    dragSize
                );
            }
            else
            {
                dragBoxFromMouseDown = Rectangle.Empty;
            }

        }

        public void selectspr()
        {
            Form4 f4 = new Form4(this);
            f4.Show();
        }

        private void lol_MouseMove(object sender, MouseEventArgs e)
        {
            if ((e.Button & MouseButtons.Left) == MouseButtons.Left)
            {
                // Only start dragging if the mouse leaves the tiny threshold box
                if (dragBoxFromMouseDown != Rectangle.Empty && !dragBoxFromMouseDown.Contains(e.Location))
                {
                    // Verify we actually have a valid item selected before starting
                    if (lol.SelectedItem != null)
                    {
                        lol.DoDragDrop(lol.SelectedItem, DragDropEffects.Move);

                        // Clear the box after drag finishes so future single clicks don't get trapped
                        dragBoxFromMouseDown = Rectangle.Empty;
                    }
                }
            }
            lol.SelectedIndex = lol.SelectedIndex;

            

        }



        // Triggered continuously while the user hovers over the ListBox during a drag
        private void lol_DragOver(object sender, DragEventArgs e)
        {
            // Indicate that we want to move the item
            e.Effect = DragDropEffects.Move;
        }

        // Triggered when the user releases the mouse button over the ListBox
        private void lol_DragDrop(object sender, DragEventArgs e)
        {
            // Convert screen coordinates to ListBox client coordinates
            Point point = lol.PointToClient(new Point(e.X, e.Y));

            // Find the index where the user dropped the item
            int targetIndex = lol.IndexFromPoint(point);

            if (e.Data.GetDataPresent(typeof(string)))
            {
                string draggedItem = (string)e.Data.GetData(typeof(string));

                if (draggedItem != null)
                {
                    // 1. Remove the item from your underlying list
                    forbox.Remove(draggedItem);

                    // 2. Insert item into its new position inside the list
                    if (targetIndex < 0 || targetIndex >= forbox.Count)
                    {
                        forbox.Add(draggedItem); // Append to end if dropped in empty space
                    }
                    else
                    {
                        forbox.Insert(targetIndex, draggedItem);
                    }

                        // Rebind the updated list

                    // 4. Maintain the active selection
                    lol.SelectedItem = draggedItem;
                    boundlist.ResetBindings();
                }
            }
        }

        public PictureBox spr;

        public PictureBox sprp;


        public Label jk;
        public System.Windows.Forms.PictureBox bg;
        public Form2 frm2;
        public Form1 frm1;
        public ListBox lol;
        public List<string> BGs = new List<string>() { @"C:\Users\enzo\Desktop\finl\finl\backgrounds\bg_stream.png", @"C:\Users\enzo\Desktop\finl\finl\backgrounds\bg_stream_angel_lv1.png", @"C:\Users\enzo\Desktop\finl\finl\backgrounds\bg_stream_angel_lv2.png", @"C:\Users\enzo\Desktop\finl\finl\backgrounds\bg_stream_angel_lv3.png", @"C:\Users\enzo\Desktop\finl\finl\backgrounds\bg_stream_angel_lv4.png", @"C:\Users\enzo\Desktop\finl\finl\backgrounds\bg_stream_angel_lv5.png", @"C:\Users\enzo\Desktop\finl\finl\backgrounds\bg_stream_kyouso.png", @"C:\Users\enzo\Desktop\finl\finl\backgrounds\bg_stream_shield_gold.png", @"C:\Users\enzo\Desktop\finl\finl\backgrounds\bg_stream_shield_silver.png", @"C:\Users\enzo\Desktop\finl\finl\backgrounds\bg_stream_mansion.png" };
        public void change(string path, bool spr)
        {
            if (spr == false)
            {
                this.bg.Image = Image.FromFile(path);
                this.bg.Name = path;
            }
            else
            {
                this.sprp.Image = Image.FromFile(path);
                sprp.Refresh();

            }


            this.jk.Text = path.Replace(@"C:\Users\enzo\Desktop\finl\finl\backgrounds\", "").Replace(".png", "").Replace("_", " ");

            this.bg.Refresh();
        }

        public void listy(string name)

        {
            boundlist.Insert(0, name.Replace(@"C:\Users\enzo\Desktop\finl\finl\backgrounds\", "").Replace(".png", "").Replace("_", " "));
            lol.Refresh();
        }
        public void SelectBG(Form1 op)
        {
            Form3 Form3 = new Form3(op);
            Form3.Show();

        }
    }
    partial class Form2
    {
        public Button b;
        public Button s;
        public void y(Form1 formy, Form2 form22)
        {
            // Usually managed automatically by the Visual Studio Designer

            this.b = new System.Windows.Forms.Button();
            this.b.Location = new System.Drawing.Point(25, 50);
            this.b.Size = new System.Drawing.Size(100, 30);
            this.b.Text = "BGs";

            // Link the click event to your method
            this.b.Click += (sender, e) =>
            {
                formy.SelectBG(formy);
            };

            this.Controls.Add(b);

            this.s = new System.Windows.Forms.Button();
            this.s.Location = new System.Drawing.Point(150, 50);
            this.s.Size = new System.Drawing.Size(100, 30);
            this.s.Text = "Sprites";

            // Link the click event to your method
            this.s.Click += (sender, e) =>
            {
                formy.selectspr();
            };

            this.Controls.Add(s);
        }


    }
    public partial class Form3
    {
        public Button a;




        public PictureBox s;
        public void t(Form1 op)
        {
            this.ClientSize = new System.Drawing.Size(300, 900);


            FlowLayoutPanel imageContainer = new FlowLayoutPanel();
            imageContainer.Dock = DockStyle.Fill;
            imageContainer.AutoScroll = true; // Adds scrollbars if images exceed window size

            this.Controls.Add(imageContainer);
            foreach (string i in op.BGs)
            {

                PictureBox k = new System.Windows.Forms.PictureBox();
                k.BackColor = Color.Transparent;
                k.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
                k.Location = new System.Drawing.Point(50, 50);
                k.Name = i;
                k.Size = new System.Drawing.Size(348, 227);
                k.Image = Image.FromFile(i);
                k.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
                k.Click += (sender, e) =>
                 {
                     op.change(i, false);
                     op.listy(i);
                 };
                imageContainer.Controls.Add(k);

            }
        }

    }
    public partial class Form4 : Form
    {
        public Form4(Form1 op)
        {
            Button am = new System.Windows.Forms.Button();
            am.Location = new System.Drawing.Point(150, 50);
            am.Size = new System.Drawing.Size(100, 30);
            am.Text = "Ame";

            // Link the click event to your method
            am.Click += (sender, e) =>
            {
                Form5 i = new Form5(true,op);
                i.Show();
            };

            this.Controls.Add(am);

            Button KA = new System.Windows.Forms.Button();
            KA.Location = new System.Drawing.Point(25, 50);
            KA.Size = new System.Drawing.Size(100, 30);
            KA.Text = "KAngel";

            // Link the click event to your method
            KA.Click += (sender, e) =>
            {
                Form5 i = new Form5(true, op);
                i.Show();
            };

            this.Controls.Add(KA);

        }
    }

    public partial class  Form5 : Form
    {
        
        public Form5(bool ame, Form1 op)
        {
            this.ClientSize = new System.Drawing.Size(300, 900);


            FlowLayoutPanel imageContaineram = new FlowLayoutPanel();
            imageContaineram.Dock = DockStyle.Fill;
            imageContaineram.AutoScroll = true; // Adds scrollbars if images exceed window size

            this.Controls.Add(imageContaineram);
            foreach (string i in op.staticspri)
            {

                op.spr = new System.Windows.Forms.PictureBox();
                op.spr.BackColor = Color.Transparent;
                op.spr.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
                op.spr.Location = new System.Drawing.Point(50, 50);
                op.spr.Name = i;
                op.spr.Size = new System.Drawing.Size(348, 227);
                op.spr.Image = Image.FromFile(i);
                op.spr.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
                op.spr.Click += (sender, e) =>
                {
                    op.change(i, true);
                };
                imageContaineram.Controls.Add(op.spr);

            }
        }
    }
}