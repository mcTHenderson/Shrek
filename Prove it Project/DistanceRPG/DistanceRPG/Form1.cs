/*
 * Project Name:    Are you still a Software Engineer?
 * Programmer:      Trent Henderson
 * Date:            8/28/2017
 * 
 * Description:     A short a game where your objective
 *                  is to reach a certain distance without
 *                  losing all of your health points.
 */
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DistanceRPG
{
    public partial class DistanceRPG : Form
    {
        // Declare variables
        int distanceInt, currentDistanceInt = 0, encounterChanceInt, encounterRateInt, escapeRateInt, textSpeedInt = 2,
            accuracyInt, totalDmgInt, strInt = 1, enemyDEFInt = 1, currentHpInt = 12, enemyHPInt = 5, gpInt, currentGPInt,
            enemiesDefeatedInt = 0, difficultyModifierInt = 1, scoreInt;
        bool combat = false;
        string nameString = "Hero", difficultyString;
        Random rnd = new Random();

        public DistanceRPG()
        {
            InitializeComponent();
        }
        public void Dialogue(string dialogueString)
        {
            // Display dialogue in a textbox depending on string input.
            dialogueTB.Text = "";
            for (int i = 0; i < dialogueString.Length; i++)
            {
                // Typewritter effect
                Invoke(new MethodInvoker(delegate { dialogueTB.AppendText(dialogueString[i].ToString()); }));
                System.Threading.Thread.Sleep(25 * textSpeedInt);
            }
        }
        private void settingsLabel_Click(object sender, EventArgs e)
        {
            // Hide title screen and show settings menu
            titleLabel.Visible = false;
            startLabel.Visible = false;
            settingsLabel.Visible = false;

            nameLabel.Visible = true;
            nameTB.Visible = true;
            textSpeedLabel.Visible = true;
            textSpeedGroupBox.Visible = true;
            confirmLabel.Visible = true;
            dialogueTB.Visible = true;

            DistanceRPG.ActiveForm.Text = "Distance RPG - " + "Settings";
        }

        private void textSpeedRadioButton1_CheckedChanged(object sender, EventArgs e)
        {
            // Determine Text speed based on selection (1 being slow and 3 being fast)
            if (textSpeedRadioButton1.Checked == true)
            {
                textSpeedInt = 3;
            } else if (textSpeedRadioButton2.Checked == true)
            {
                textSpeedInt = 2;
            } else if (textSpeedRadioButton3.Checked == true)
            {
                textSpeedInt = 1;
            }
            Dialogue("Text Speed Test 1 2 3...");
        }

        private void confirmLabel_Click(object sender, EventArgs e)
        {
            // Confirm new seetings, hide settings menu, and show title screen
            titleLabel.Visible = true;
            startLabel.Visible = true;
            settingsLabel.Visible = true;

            nameLabel.Visible = false;
            nameTB.Visible = false;
            textSpeedLabel.Visible = false;
            textSpeedGroupBox.Visible = false;
            confirmLabel.Visible = false;
            dialogueTB.Visible = false;

            nameString = nameTB.Text;
        }
        private void startLabel_Click(object sender, EventArgs e)
        {
            // Hide Main menu options and show the difficulty menu.
            titleLabel.Visible = false;
            startLabel.Visible = false;
            settingsLabel.Visible = false;

            difficultyGroupBox.Visible = true;
            totalDistanceLabel.Visible = true;
            totalDistanceTB.Visible = true;
            encounterRateLabel.Visible = true;
            encounterRateTB.Visible = true;
            continueLabel.Visible = true;

            DistanceRPG.ActiveForm.Text = "Distance RPG - " + "Difficulty Menu";
        }

        private void easyRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            // Change distance and encounter rate depending on difficulty selection
            if (easyRadioButton.Checked == true)
            {
                distanceInt = 20;
                encounterRateInt = 20;
                difficultyModifierInt = 1;
                difficultyString = "Easy (x1)";
            }
            else if (mediumRadioButton.Checked == true)
            {
                distanceInt = 35;
                encounterRateInt = 30;
                difficultyModifierInt = 2;
                difficultyString = "Medium (x2)";
            }
            else if (difficultRadioButton.Checked == true)
            {
                distanceInt = 50;
                encounterRateInt = 40;
                difficultyModifierInt = 3;
                difficultyString = "Difficult (x3)";
            }
            else if (customRadioButton.Checked == true)
            {
                totalDistanceTB.ReadOnly = false;
                encounterRateTB.ReadOnly = false;
                difficultyModifierInt = 1;
                difficultyString = "Custom (1x)";
            }
            totalDistanceTB.Text = distanceInt.ToString();
            encounterRateTB.Text = encounterRateInt.ToString();
        }

        private void continueLabel_Click(object sender, EventArgs e)
        {
            // Check difficulty selection and values
            try
            {
                distanceInt = int.Parse(totalDistanceTB.Text);
            }
            catch
            {
                if (customRadioButton.Checked == true)
                {
                    MessageBox.Show("Distance must be greater than zero", "Distance Data Error 1", MessageBoxButtons.OK);
                    return;
                }
                else
                {
                    MessageBox.Show("Please Select a difficulty", "No Difficulty Selected", MessageBoxButtons.OK);
                    return;
                }
            }
            try
            {
                encounterRateInt = int.Parse(encounterRateTB.Text);
            }
            catch
            {
                if (customRadioButton.Checked == true)
                {
                    MessageBox.Show("Please specify the encounter rate", "Encounter Rate Data Error", MessageBoxButtons.OK);
                    return;
                }
                else
                {
                    MessageBox.Show("Please Select a difficulty", "No Difficulty Selected", MessageBoxButtons.OK);
                    return;
                }
            }
            if (distanceInt < 1)
            {
                MessageBox.Show("Distance must be greater than zero", "Distance Data Error 2", MessageBoxButtons.OK);
                return;
            }
            if (encounterRateInt > 100 || encounterRateInt < 0)
            {
                MessageBox.Show("Encounter rate must be equal or between 0 and 100", "Encounter Rate Data Error 2");
                return;
            }
            // Hide difficulty menu
            difficultyGroupBox.Visible = false;
            totalDistanceLabel.Visible = false;
            totalDistanceTB.Visible = false;
            encounterRateLabel.Visible = false;
            encounterRateTB.Visible = false;
            continueLabel.Visible = false;
            // Show Adventure menu and display health points, gold pieces, and distance
            dialogueTB.Visible = true;
            tutorialLabel.Visible = true;
            continueLabel2.Visible = true;

            currentHPLabel.Text = currentHpInt.ToString();
            hpLabel.Visible = true;
            currentHPLabel.Visible = true;

            currentGPLabel.Text = gpInt.ToString();
            gpLabel.Visible = true;
            currentGPLabel.Visible = true;

            currentLabel.Text = currentDistanceInt.ToString();
            currentLabel.Visible = true;
            slashLabel.Visible = true;
            totalLabel.Text = distanceInt.ToString();
            totalLabel.Visible = true;
            // Display tutorial text
            DistanceRPG.ActiveForm.Text = "Distance RPG - " + "Tutorial";
            Dialogue("Welcome to Distance RPG! In this game your objective is to get to the very end..." +
                " Without dying of course because the paths are dangerous and riddled with monsters. Choose your actions" +
                " wisely and with a little luck you can achieve your goal!");

        }

        private void continueLabel2_Click(object sender, EventArgs e)
        {
            // Reset title, hide tutorial (if opened), and call continue function
            DistanceRPG.ActiveForm.Text = "Distance RPG - " + "Adventure";
            tutorialLabel.Visible = false;
            Continue();
        }

        public void Continue()
        {
            // Increase distance traveled and check if there is an encounter.
            if (currentDistanceInt < distanceInt)
            {
                // Reset encounter chance, display dialogue and increase distance traveled
                encounterChanceInt = 0;
                Dialogue(nameString + " Continues...");
                currentDistanceInt++;
                currentLabel.Text = currentDistanceInt.ToString();

                // Get a random number (between 0-100) that determines if there is an encounter.
                encounterChanceInt = rnd.Next(0, 100);

                // Message box testing
                    //MessageBox.Show("Encounter Chance: " + encounterChanceInt + "\n"
                    //    + "Encounter Rate: " + encounterRateDecimal);

                // If encounter test succeeds start a battle
                if (encounterChanceInt <= encounterRateInt)
                {
                    dialogueTB.Text = "";
                    continueLabel2.Visible = false;
                    combat = true;
                    enemyHPInt = 5;
                    Dialogue(nameString + " is ambushed! Battle Start!");
                    combatGroupBox.Visible = true;
                }
                else
                {
                    dialogueTB.Text = "";
                    Dialogue("Nothing Happens...");
                }
            } else
            {
                // Hide main game elements (except dialogue box)
                continueLabel2.Visible = false;
                hpLabel.Visible = false;
                currentHPLabel.Visible = false;
                gpLabel.Visible = false;
                currentGPLabel.Visible = false;
                currentLabel.Visible = false;
                slashLabel.Visible = false;
                totalDistanceLabel.Visible = false;

                // Calculate final rank and summary values.
                scoreInt = ((currentDistanceInt * 5) + (enemiesDefeatedInt * 20) + (currentHpInt * 20)) * difficultyModifierInt;
                floorsClearedLabel.Text = currentDistanceInt.ToString();
                    //(future update) totalTreasureLabel.Text = 
                enemiesDefeatedLabel.Text = enemiesDefeatedInt.ToString();
                healthRemainingLabel.Text = currentHpInt.ToString();
                scoreLabel.Text = scoreInt.ToString();
                difficultyClearedLabel.Text = difficultyString;

                // Display summary variables
                victoryLabel.Visible = true;
                summaryLabel.Visible = true;
                floorsClearedLabel.Visible = true;
                    //(future update)totalTreasureLabel.Visible = true;
                enemiesDefeatedLabel.Visible = true;
                healthRemainingLabel.Visible = true;
                difficultyClearedLabel.Visible = true;
                scoreLabel.Visible = true;
                DistanceRPG.ActiveForm.Text = "Distance RPG - " + "Summary";
                Dialogue(nameString + " successfully made it to the end! (Good Ending)");
            }
        }
        // Possible options in future updates for the combat screen
        private void magicLabel_Click(object sender, EventArgs e)
        {
            Dialogue("Sorry but you don't have this option... Unlock this in the Give Meh Monies DLC");
        }
        // Check if the player escapes or not.
        private void runLabel_Click(object sender, EventArgs e)
        {
            //check if the player is able to escape
            combatGroupBox.Visible = false;
            escapeRateInt = rnd.Next(0, 100);
            Dialogue(nameString + " tries to escape...");
            // Remove gold if available.
            if (currentGPInt > 0)
            {
                gpInt = 0;
                gpInt = rnd.Next(1, 6);
                currentGPInt -= gpInt;
                if (currentGPInt < 0)
                {
                    currentGPInt = 0;
                } else
                {
                    Dialogue(nameString + " loses some gold pieces in the struggle...");
                }
                currentGPLabel.Text = currentGPInt.ToString();
            }
            // Player escapes
            if (escapeRateInt >= 60)
            {
                combat = false;
                combatGroupBox.Visible = false;
                continueLabel2.Visible = true;
                Dialogue(nameString + " escapes!");
            } else
            {
                // Player fails to escape and transitions to enemy turn
                Dialogue(nameString + " couldnt escape!");
                EnemyTurn();
            }
        }
        public void EnemyTurn()
        {
            // Check enemy health if > 0 continue enemies turn
            if (enemyHPInt > 0)
            {
                Dialogue("The enemy attacks...");
                // pick a random number(0-100) for enemy accuracy
                accuracyInt = rnd.Next(0, 100);
                // Check accuracy integer if >= 30 deal damage to the player
                if (accuracyInt >= 30)
                {
                    // pick a number(1-3) for damage dealt to the player
                    totalDmgInt = rnd.Next(1, 3);
                    Dialogue("The enemy's attack hits and deals " + totalDmgInt + " damage!");
                    currentHpInt = currentHpInt - totalDmgInt;
                    currentHPLabel.Text = currentHpInt.ToString();
                }
                else
                {
                    Dialogue("The enemy's attack missed!");
                }
            }
            else
            {
                // return to the Adventure screen after the enemy is slain
                Dialogue("The enemy is defeated!");
                combat = false;
                combatGroupBox.Visible = false;
                enemiesDefeatedInt++;
                gpInt = 0;
                gpInt = rnd.Next(1, 11);
                currentGPInt += gpInt;
                currentGPLabel.Text = currentGPInt.ToString();
                Dialogue(nameString + " found " + gpInt + " gold pieces!");
                continueLabel2.Visible = true;

            }
            // change the color of health points to red
            if (currentHpInt <= 3)
                currentHPLabel.ForeColor = System.Drawing.Color.Red;

            // display the Game over screen if health points goes lower than 1
            if (currentHpInt <= 0)
            {
                combat = false;
                hpLabel.Visible = false;
                currentHPLabel.Visible = false;
                gpLabel.Visible = false;
                currentGPLabel.Visible = false;
                currentLabel.Visible = false;
                slashLabel.Visible = false;
                totalLabel.Visible = false;
                combatGroupBox.Visible = false;

                gameOverLabel.Visible = true;

                DistanceRPG.ActiveForm.Text = "Distance RPG - " + "Game Over";
                Dialogue(nameString + " was not able to survive the journey... (Bad Ending)");
            }
            else
            {
                if (combat == true)
                {
                    combatGroupBox.Visible = true;
                }
            }
        }
        private void attackLabel_Click(object sender, EventArgs e)
        {
            // Check accuracy, damage, enemy accuracy, enemy damage, current health points, and enemy health points
            combatGroupBox.Visible = false;
            Dialogue(nameString + " attacks...");

            // Get a random number for accuracy (0-100)
            accuracyInt = rnd.Next(0, 100);

            // Check accuracy integer if >= 20 deal damage, if >= 90 deal a critical hit
            if (accuracyInt >= 20)
            {
                if (accuracyInt >= 90)
                {
                    // pick a random number(1-6) for damage, add modifiers, and multiply by 2
                    totalDmgInt = ((rnd.Next(1, 6) + strInt) - enemyDEFInt) * 2;
                    Dialogue(nameString + " landed a critical hit and deals " + totalDmgInt + " damage!");
                }
                else
                {
                    // pick a random number(1-6) for damage and add modifiers
                    totalDmgInt = (rnd.Next(1, 6) + strInt) - enemyDEFInt;
                    Dialogue(nameString + "'s attack hits and deals " + totalDmgInt + " damage!");
                }
                enemyHPInt = enemyHPInt - totalDmgInt;
            }
            else
            {
                Dialogue(nameString + "'s attack missed!");
            }
            EnemyTurn();
        }
    }
}
