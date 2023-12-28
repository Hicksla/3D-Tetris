using System;

namespace pointSystem{
	public class Points{
		private int points;
		private int level;

		private int[] highScores;
		private string[] highScoreNames;

		public void init(){
			points = 0;
			level = 0;
			highScores = new int[5];
			highScoreNames = new string[5];
        }

		public void levelUp(){

        }
		public void addPoints(int pointAmount){
			points += pointAmount;
		}
	}
}
