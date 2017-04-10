using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MudrakPatel_Test02_Sec003_Ex1
{
    public partial class BaseballForm : Form
    {
        public BaseballForm()
        {
            InitializeComponent();
        }

        public MudrakPatel_Test02_Sec003_ClassLibrary.BaseballEntities dbcontext = new MudrakPatel_Test02_Sec003_ClassLibrary.BaseballEntities();

        private void BaseballForm_Load(object sender, EventArgs e)
        {
            //Load the data from dbcontext on the Load event
            dbcontext.Players
                .OrderBy(player => player.PlayerID)
                .ThenBy(player => player.FirstName)
                .ThenBy(player => player.LastName)
                .Load();
            //Provide DataSource to the playerBindingSource
            playerBindingSource.DataSource = dbcontext.Players.Local.ToBindingList();
        }

        private void playerBindingNavigatorSaveItem_Click(object sender, EventArgs e)
        {
            //Validate for unsaved changes
            Validate();
            //Save changes to the database
            dbcontext.SaveChangesAsync();
            //Refresh the playerDataGridView
            playerDataGridView.Refresh();
        }

        private void firstNameSearchButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (firstNameTextBox.Text != "")
                {
                    var playersByFirstName = from player in dbcontext.Players
                                             where player.FirstName.Equals(firstNameTextBox.Text)
                                             select player;

                    var details = "";
                    foreach (var player in playersByFirstName)
                    {
                        details = details + "<<" + player.FirstName + ">>"
                                          + "<<" + player.LastName + ">>"
                                          + "<<" + player.PlayerID + ">>"
                                          + Environment.NewLine;
                    }
                    MessageBox.Show(details, "--Searched Players--");

                    if (playersByFirstName.Any() || playersByFirstName.Equals(null))
                    {
                        MessageBox.Show("The player with provided first name doesn't exist!" + Environment.NewLine + "Check the first name that you entered!", "Validation error!");
                    }

                } else
                {
                    MessageBox.Show("First name textbox should not be empty!", "Validation error!");
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "Exception caught!");
            }
        }

        private void battingAverageSearchTextBox_Click(object sender, EventArgs e)
        {
            //Temporary variables to hold
            //min and max batting average values
            var minValue = 0.000m;
            var maxValue = 1.000m;

            try
            {

                if ((minValueTextBox.Text.Equals("") || minValueTextBox.Text.Equals(null))
                    || (maxValueTextBox.Text.Equals("") || maxValueTextBox.Text.Equals(null)))
                {
                    MessageBox.Show("Please fill in both textboxes." + Environment.NewLine + 
                        "Negative values are not allowed!", "Validation error!");
                }
                else
                {
                    if (minValue < 0.000m) { minValue = 0.000m; }
                    else { minValue = Convert.ToDecimal(minValueTextBox.Text); }

                    if (maxValue > 1.000m) { maxValue = 1.000m; }
                    else { maxValue = Convert.ToDecimal(maxValueTextBox.Text); }

                    var selectedPlayer = from player in dbcontext.Players
                                         where (player.BattingAverage >= minValue
                                                && player.BattingAverage <= maxValue)
                                         select player;

                    var details = "";
                    foreach (var player in selectedPlayer)
                    {
                        details = details + "<<" + player.PlayerID + ">> "
                                          + "<<" + player.FirstName + ">> "
                                          + "<<" + player.LastName + ">> "
                                          + "<<" + player.BattingAverage + ">> "
                                          + Environment.NewLine;
                    }
                    MessageBox.Show(details, "--Searched Players--");
                }
            }
            catch (ArgumentException exception)
            {
                MessageBox.Show(exception.Message, "Exception occured!");
            }
            catch (Exception exception)
            {
                MessageBox.Show("The player you are looking for might not exist in the database."
                                + Environment.NewLine + "OR" + "Check the batting average you entered."
                                + Environment.NewLine + exception.Message, "Exception or input error!");
            }
        }

        private void browseAllButton_Click(object sender, EventArgs e)
        {
            try
            {
                var allDetails = dbcontext.Players;
                var details = "";
                foreach (var player in allDetails)
                {
                    details = details + "<<" + player.PlayerID + ">> "
                                      + "<<" + player.FirstName + ">> "
                                      + "<<" + player.LastName + ">> "
                                      + "<<" + player.BattingAverage + ">> "
                                      + Environment.NewLine;
                }
                MessageBox.Show(details, "--Searched Players--");
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "Exception caught!");
            }
        }

        private void highestBattingAverageButton_Click(object sender, EventArgs e)
        {
            var highestAverage = dbcontext.Players.Distinct().Max(playerHold => playerHold.BattingAverage);
            var details = "";
            var selectedPlayer = (from player in dbcontext.Players
                                 where player.BattingAverage.Equals(highestAverage)
                                 select player).Distinct();

            foreach (var player in selectedPlayer)
            {
                details = details + "<<" + player.PlayerID + ">> "
                                  + "<<" + player.FirstName + ">> "
                                  + "<<" + player.LastName + ">> "
                                  + "<<" + player.BattingAverage + ">> "
                                  + Environment.NewLine;
            }

            MessageBox.Show(details, "--Searched Players--");

        }

        private void sortByFirstNameButton_Click(object sender, EventArgs e)
        {
            //Load the data from dbcontext on the Load event
            dbcontext.Players
                .OrderBy(player => player.FirstName)
                .ThenBy(player => player.LastName)
                .ThenBy(player => player.PlayerID)
                .Load();

            playerBindingSource.DataSource = dbcontext.Players.Local.ToBindingList();
        }
    }
}
