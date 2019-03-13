using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace BugsBugs
{
    public partial class mainForm : Form
    {
        public mainForm()
        {
            InitializeComponent();
        }

        private void btnSearch_Click(object sender, EventArgs e) {
            List<Album> result = WebAgent.Search(WebAgent.SEARCHTYPE.MUSIC, txtBoxKeyword.Text);
            DisplaySearchResult(result);
        }

        private void btnTop100_Click(object sender, EventArgs e) {
            List<Album> result = WebAgent.Top100();
            DisplaySearchResult(result);
        }

        private void DisplaySearchResult(List<Album> result) {
            //
        }
    }
}
