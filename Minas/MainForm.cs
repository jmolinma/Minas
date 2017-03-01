﻿/*
 * Created by SharpDevelop.
 * User: jmolinma
 * Date: 15/02/2017
 * Time: 17:45
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;

using System.Diagnostics;
using System.Windows.Forms;

namespace Minas
{
	public partial class MainForm : Form
	{
		private Tablero tablero;

        private const string NICKNAME_MESSAGE = "Player";

		private const string WINNER_CAPTION = "WINNER!";
		private const string WINNER_MESSAGE = "¡Has ganado! ¿Deseas volver a jugar?";
		
		private const string LOOSER_CAPTION = "LOOSER!";
		private const string LOOSER_MESSAGE = "¡Has perdido! ¿Deseas volver a jugar?";
		
		private const string HIGHT_DIFFICULTY = "Alta";
		private const string MEDIUM_DIFFICULTY = "Media";
		private const string LOW_DIFFICULTY = "Baja";
		
		private const int HIGHT_DIFFICULTY_INT = 30;
		private const int MEDIUM_DIFFICULTY_INT = 15;
		private const int LOW_DIFFICULTY_INT = 7;
		
		private const string LARGE_SIZE = "Grande";
		private const string MEDIUM_SIZE = "Mediano";
		private const string SMALL_SIZE = "Pequeño";
		
		private const int LARGE_SIZE_INT = 10;
		private const int MEDIUM_SIZE_INT = 8;
		private const int SMALL_SIZE_INT = 5;
		
		private int numColumns;
		private int difficulty;
        private string nickName;
		
		public MainForm()
		{
			UpdateVariables(10, 15);
			InitializeComponent(numColumns);
			AddListeners();
			UpdateForm();
		}

		private void UpdateVariables(int numColumn, int difficulty)
		{
            this.nickName = "Player";
			this.numColumns = numColumn;
			this.difficulty = difficulty;
			this.tablero = new Tablero(numColumns, numColumns, difficulty);
		}
		
		private void UpdateForm(){
			this.tablero.updateTablero(buttons);
			this.labelNumeroBombas.Text = "Numero de bombas: "+ tablero.getNumeroBombas(); 
		}
		
		private void ComboBoxesSelectedIndexChanged(ComboBox comboBox)//ComboBox difficulties & 
		{
			string selectedItem = comboBox.SelectedItem.ToString();
			
			switch (selectedItem)
            {
				case LARGE_SIZE:
					this.numColumns = LARGE_SIZE_INT;
                    break;
				case MEDIUM_SIZE:
                    this.numColumns = MEDIUM_SIZE_INT;
                    break;
				case SMALL_SIZE:
                  	this.numColumns = SMALL_SIZE_INT;
                   	break;
				case HIGHT_DIFFICULTY:
                   	this.difficulty = HIGHT_DIFFICULTY_INT;
                  	break;
				case MEDIUM_DIFFICULTY:
                    this.difficulty = MEDIUM_DIFFICULTY_INT;
                    break;
                case LOW_DIFFICULTY:
                   	this.difficulty = LOW_DIFFICULTY_INT;
                   	break;
			}
			UpdateVariables(this.numColumns, this.difficulty);
			UpdateComponent(numColumns);
			AddListeners();
			UpdateForm();
		}
		
		private void ButtonsClick(System.Windows.Forms.Button btn, object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Right){
				PutFlag(btn);
			}else{
				String name = btn.Name;
				int f = Int32.Parse(name.Substring(0, 2));
				int c = Int32.Parse(name.Substring(2,2));
				if(tablero.levanta(f, c)){
                   	UpdateForm();
					ShowLooserMessage();
				}
			}
			UpdateForm();
		}

		private void PutFlag(Button btn)
		{
			if(!tablero.ponerBandera(btn)){
				UpdateForm();
				ShowLooserMessage();
			}else{
				checkWinner();
			}
		}
		
		private void checkWinner(){
			if(tablero.getNumeroBombas() == 0){
				UpdateForm();
				ShowWinnerMessage();
			}
		}
		
		private void ShowWinnerMessage(){
            ShowOptionMessage(WINNER_MESSAGE, WINNER_CAPTION);
            AddNewRecord();
        }

        private void AddNewRecord()
        {

            ScoreEntities scoreEntities = new ScoreEntities();
            Score score = new Score();
            score.Nick = this.textBoxNickName.Text;
            score.Bombs = tablero.getNumeroBombas();
            score.Difficulty = this.difficulty;
            scoreEntities.Scores.Add(score);
            scoreEntities.SaveChanges();
            
        }

		private void ShowLooserMessage(){
			ShowOptionMessage(LOOSER_MESSAGE, LOOSER_CAPTION);
            AddNewRecord();
        }
		
		private void ShowOptionMessage(string message, string caption){
			MessageBoxButtons options = MessageBoxButtons.YesNo;
			DialogResult result;
			result = MessageBox.Show(message, caption, options);
			if (result == System.Windows.Forms.DialogResult.Yes){
				this.tablero = new Tablero(numColumns, numColumns, difficulty);
				UpdateForm();
			}else{
				this.Close();
			}
		}
	}
}
