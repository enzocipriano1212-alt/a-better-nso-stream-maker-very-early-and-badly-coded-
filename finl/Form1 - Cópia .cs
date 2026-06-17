using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace finl
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            
            // change(@"C:\Users\enzo\Desktop\prj 7\oi\game\mod_assets\ame\stream_ame_craziness_001.png");
            Form2 Form2 = new Form2(this);
            Form2.Show();
            
            
        }
    }
    public partial class Form2 : Form
    {
        public Form1 frm1; // Class-level variable

        public Form2(Form1 op)
        {

            // Remove the word 'Form1' from the front of this line!
            frm1 = op;
            y(frm1, this);


            // Note: You must call frm1.Show(); somewhere if you want to actually see this new form.
        }
    }
    public partial class Form3 : Form
    {
        public Form3(Form1 op)
        {

            t(op);   
        }
    }

}

