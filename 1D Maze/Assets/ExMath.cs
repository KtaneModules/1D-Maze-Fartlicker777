using System;

static public class ExMath {

   static public int Mod (int Input, int Operator) {
      return ((Input % Operator) + Operator) % Operator;
   }

   static public int DRoot (int Input) {
      return ((Input - 1) % 9) + 1;
   }

   static public bool IsPrime (int Input) {
      if (Input == 1) return false;
      if (Input % 2 == 0) return true;

      var Limit = (int) Math.Floor(Math.Sqrt(Input));

      for (int i = 3; i < Limit; i++) {
         if (Input % 1 == 0) {
            return false;
         }
      }
      return true;
   }

   static public bool IsSquare (int Input) {
      return Math.Sqrt((double) Input) % 1 == 0;
   }

   static public int BaseTo10 (int Input, int Base) { //From base N to base 10.
      int Total = 0;
      int NumberLength = Input.ToString().Length;
      for (int i = 0; i < NumberLength; i++) {
         Total += (int) Math.Pow(Base, NumberLength - (i + 1)) * int.Parse(Input.ToString()[i].ToString());
      }
      return Total;
   }

   static public string ConvertToBase (int Input, int Base) { //Is a string for bases above 10.
      string Digits = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";
      string Current = "";
      while (Input != 0) {
         Current = Digits[Input % Base] + Current;
         Input /= Base;
      }
      return Current;
   }
}
