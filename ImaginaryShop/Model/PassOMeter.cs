namespace ImaginaryShop.Model
{
    public class PassOMeter
    {
        int points = 0;
        public PassOMeterResult Validate(string password)
        {
            PassOMeterResult pr = new PassOMeterResult();
            //passwordet skal eksisterer og længden skal være minimum 12 tegn
            if ((password == null) || password.Trim().Length < 12) {

                pr.Score = 0;
                pr.Message = "Passwordet er ikke langt nok";
                return pr;
            }
               
            
            //Check om der er uppercase, hvis ja giv 10 point. Hvis første tegn er uppercase, så træk 5 fra
            int uppercaseCount = password.Count(char.IsUpper);
            if ((uppercaseCount == 1) && (char.IsUpper(password[0])))
            {
                pr.Score += 5;
                pr.Message = "Du bør ikke lade det første tegn i dit password være stort begyndelsesbogstav";
            }
            else
                pr.Score += 10;

               
        
            return pr;
        }
    }

    public class PassOMeterResult
    {
        int score =0;
        string message;

        public int Score { get => score; set => score = value; }
        public string Message { get => message; set => message = value; }
    }
}
