using Project.Data.Enteties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project.Templates
{
    public static class Templates
    {
        public static string ReportLetter(string surname, string name, string email, int userId, int year, int month)
        {
            return $"<!DOCTYPE html><html lang=\"en\"><head> <meta charset=\"UTF-8\"> <title>Title</title></head><body> <div style=\"border: 1px solid black; width: 440px; height: auto;\"> <div style=\"border-bottom: 1px solid #D5D5D5; background-color: #F9F9F9; display: flex; align-items: center; justify-content :center;\"> <br> <div style=\"width: 150px; height: 65px; display: flex; align-items: center;\"><img style=\"width: 100%; height: auto\"src=\"https://i.ibb.co/myXhkPp/logo.png\" alt=\"logo\"></div> <br> </div> <div style=\"border-bottom: 1px solid #D5D5D5; text-align: center\"> <h2>Notification</h2><br> <p>{surname} {name} (manager email: <a>{email}</a>) has submitted a timesheet. </div> <div style=\"display: flex; align-items: center; justify-content :center; height: 50px; background-color: #F9F9F9\"> <a href=\"http://localhost:5000/api/letterreport/table/{userId}/{year}/{month}\"><input type=\"button\" style=\"background-color: #ff7e02; color: white; padding: 5px 12px; text-align: center; display: inline-block; font-size: 16px; cursor: pointer\" name=\"accept\" value=\"View\"></input></a> </div> </div></body></html>";
        }

        public static string ManagerLetter(
            string email, string name, string surname, string login, int hours, int userId, int year, int month)
        {
            return $"<!DOCTYPE html><html lang = \"en\"><head><meta charset = \"UTF-8\"><title> Title </title></head><body><div style = \"border: 2px solid black; width: 440px; height: auto;\"><div style = \"border-bottom: 1px solid #D5D5D5; background-color: #F9F9F9; display: flex; align-items: center; justify-content :center;\"><br><div style = \"width: 150px; height: 65px; display: flex; align-items: center;\"><img style = \"width: 100%; height: auto\"src = \"https://i.ibb.co/myXhkPp/logo.png\" alt = \"logo\"></div><br></div><div style = \"border-bottom: 1px solid #D5D5D5\"><p><strong> Dear {email},</strong><p><br><br><p> Your subordinate {surname} {name}(login: <a> {login}</a>) has worked {hours} hours.</div><div style = \"display: flex; align-items: center; justify-content :center; height: 50px; background-color: #F9F9F9\"> <a href=\"http://localhost:5000/api/letterreport/Notify/{userId}/{year}/{month}/1\"><input type = \"button\" style = \"background-color: #4CAF50; color: white; padding: 5px 12px; text-align: center; display: inline-block; font-size: 16px; cursor: pointer\"name = \"accept\" value = \"Accept\"></a></input><a href=\"http://localhost:5000/api/account/Notify/{userId}/{year}/{month}/0\"><input type = \"button\" style = \"background-color: #f44336; color: white; padding: 5px 12px; text-align: center; display: inline-block; font-size: 16px; cursor: pointer\"name = \"decline\" value = \"Decline\"></input></a></div></div></body></html>";
        }

        public static string ContractorTable(int month, int year, List<Timesheet> items)
        {
            int days = DateTime.DaysInMonth(year, month);
            string first = "<!DOCTYPE html><html lang=\"en\"><head> <meta charset=\"UTF-8\"> <title>Contractor</title> <link rel=\"stylesheet\" href=\"https://stackpath.bootstrapcdn.com/bootstrap/4.3.1/css/bootstrap.min.css\" integrity=\"sha384-ggOyR0iXCbMQv3Xipma34MD+dH/1fQ784/j6cY/iJTQUOhcWr7x9JvoRxT2MZw1T\" crossorigin=\"anonymous\"> <style type=\"text/css\"> header{ border-bottom: 1px solid lightgray;}footer{ background-color: black;}.tableholder{ margin: 0 10px;}#buttonholder{ display: flex; justify-content: center; position: fixed; bottom: 0; z-index: 99999999; width: 100%; background-color: rgba(211, 211, 211, 0.2); height: 70px;}.btn{ margin: 10px; width: 145px;}.btn + .btn{ margin-left: 30px;}.save{ background-color: #8FCB85 !important; color: white !important;}.save:hover{ background-color: #33C13C !important;}input[readonly].form-control{ width: 10%; background-color:transparent; font-size: 1em;}tr:hover {background-color: #F5E3C5 !important;}th{ text-align: center; background-color: #FF8800; color: white;}.center{ display: flex; align-items: center;}.center .ct{ flex: 1; display: flex; justify-content: center;}.iconholder{ display: flex; align-items: center; margin-right: 10%;}.iconholder .logout{ margin-left: 25px;}.iconholder div .btn{ padding: 10px; width: 100px; font-size: 14px;}h1, h4{ font-family: 'Exo', sans-serif;}.hicon{ margin: 20px; width: 40px; height: 40px;}.logo{ /*display: inline-block;*/ margin-top: 20px; margin-bottom: 20px; margin-left: 10%; height: 60px; z-index: 100;}.dropdown { position: relative; display: inline-block;}.dropdown-content { display: none; position: absolute; background-color: #f1f1f1; min-width: 230px; max-height: 195px; overflow: auto; box-shadow: 0px 8px 16px 0px rgba(0,0,0,0.2); z-index: 1;}.dropdown-content a { color: black; padding: 12px 16px; text-decoration: none; display: block;}.dropdown-content a:hover {background-color: #ddd;}.dropdown:hover .dropdown-content {display: block;}.email{ text-align: center;}.email input[type='button']{ padding: 7px;}.email input[type='text']{ width: 90%; heigth: 30px;}.notifications a{ border-bottom: 1px solid #D4D1CD;}.dot { height: 15px; width: 15px; border-radius: 50%; border: 0.3px solid #D4D1CD; display: inline-block;}.app .dot{ background-color: green;}.napp .dot{ background-color: red;}.dot:after { content: \"\00a0\";}td, th{ text-align: center; vertical-align: middle !important;}/* footer */footer{ height: 70px; display: flex; justify-content: flex-end; align-items: center;}footer .ft{ margin-right: 40px;}footer p{ font-size: 16px; margin-bottom: 0px !important;}/* endfooter *//* buttons */.srcont{ display: flex; justify-content: space-evenly;}.tdbutton{ background-color: #F3F0F0; font-size: 14px; padding: 5px 10px; border-radius: 12px;}/* endbuttons */.wedit{ width: 100%;}.badge { display: none; padding: 4px 7px; position: absolute; top: 50px; right: 8px; border-radius: 50%; border: 0.3px solid #D4D1CD; background: red; color: white;} </style></head><body> <div class=\"tableholder\"> <table class=\"table table-striped table-bordered\" id='timesheet' border=\"1\"> <thead> <th>Day</th> <th>Date</th> <th>Billable Hours</th><th>Sick Days</th><th>Holidays</th><th>Time Off</th><th>Special Leave</th> <th>Extra Additional KMs' for Contractor</th> <th>Comments</th> </thead> <tbody id=\"tablebody\">";
            string last = "</tbody></table></div></body></html>";
            string main = "";
            for (int i = 1; i <= days; i++)
            {
                var item = items.FirstOrDefault(x => x.Day == i && x.Year == year && x.Month == month);
                main += ContractorTableRow(new DateTime(year, month, i), item);
            }

            return first + main + last;
        }

        public static string WorkerTable(int month, int year, List<Timesheet> items)
        {
            int days = DateTime.DaysInMonth(year, month);
            string first = "<!DOCTYPE html><html lang=\"en\"><head> <meta charset=\"UTF-8\"> <title>Worker</title> <link rel=\"stylesheet\" href=\"https://stackpath.bootstrapcdn.com/bootstrap/4.3.1/css/bootstrap.min.css\" integrity=\"sha384-ggOyR0iXCbMQv3Xipma34MD+dH/1fQ784/j6cY/iJTQUOhcWr7x9JvoRxT2MZw1T\" crossorigin=\"anonymous\"> <style type=\"text/css\"> header{ border-bottom: 1px solid lightgray;}footer{ background-color: black;}.tableholder{ margin: 0 10px;}#buttonholder{ display: flex; justify-content: center; position: fixed; bottom: 0; z-index: 99999999; width: 100%; background-color: rgba(211, 211, 211, 0.2); height: 70px;}.btn{ margin: 10px; width: 145px;}.btn + .btn{ margin-left: 30px;}.save{ background-color: #8FCB85 !important; color: white !important;}.save:hover{ background-color: #33C13C !important;}input[readonly].form-control{ width: 10%; background-color:transparent; font-size: 1em;}tr:hover {background-color: #F5E3C5 !important;}th{ text-align: center; background-color: #FF8800; color: white;}.center{ display: flex; align-items: center;}.center .ct{ flex: 1; display: flex; justify-content: center;}.iconholder{ display: flex; align-items: center; margin-right: 10%;}.iconholder .logout{ margin-left: 25px;}.iconholder div .btn{ padding: 10px; width: 100px; font-size: 14px;}h1, h4{ font-family: 'Exo', sans-serif;}.hicon{ margin: 20px; width: 40px; height: 40px;}.logo{ /*display: inline-block;*/ margin-top: 20px; margin-bottom: 20px; margin-left: 10%; height: 60px; z-index: 100;}.dropdown { position: relative; display: inline-block;}.dropdown-content { display: none; position: absolute; background-color: #f1f1f1; min-width: 230px; max-height: 195px; overflow: auto; box-shadow: 0px 8px 16px 0px rgba(0,0,0,0.2); z-index: 1;}.dropdown-content a { color: black; padding: 12px 16px; text-decoration: none; display: block;}.dropdown-content a:hover {background-color: #ddd;}.dropdown:hover .dropdown-content {display: block;}.email{ text-align: center;}.email input[type='button']{ padding: 7px;}.email input[type='text']{ width: 90%; heigth: 30px;}.notifications a{ border-bottom: 1px solid #D4D1CD;}.dot { height: 15px; width: 15px; border-radius: 50%; border: 0.3px solid #D4D1CD; display: inline-block;}.app .dot{ background-color: green;}.napp .dot{ background-color: red;}.dot:after { content: \"\00a0\";}td, th{ text-align: center; vertical-align: middle !important;}/* footer */footer{ height: 70px; display: flex; justify-content: flex-end; align-items: center;}footer .ft{ margin-right: 40px;}footer p{ font-size: 16px; margin-bottom: 0px !important;}/* endfooter *//* buttons */.srcont{ display: flex; justify-content: space-evenly;}.tdbutton{ background-color: #F3F0F0; font-size: 14px; padding: 5px 10px; border-radius: 12px;}/* endbuttons */.wedit{ width: 100%;}.badge { display: none; padding: 4px 7px; position: absolute; top: 50px; right: 8px; border-radius: 50%; border: 0.3px solid #D4D1CD; background: red; color: white;} </style></head><body> <div class=\"tableholder\"> <table class=\"table table-striped table-bordered\" id='timesheet' border=\"1\"> <thead> <th>Day</th> <th>Date</th> <th>Billable Hours</th> <th>Extra Additional KMs' for Contractor</th> <th>Comments</th> </thead> <tbody id=\"tablebody\">";
            string last = "</tbody></table></div></body></html>";
            string main = "";
            for(int i = 1; i <= days; i++)
            {
                var item = items.FirstOrDefault(x => x.Day == i && x.Year == year && x.Month == month);
                main += WorkerTableRow(new DateTime(year, month, i), item);
            }

            return first + main + last;
        }

        private static string WorkerTableRow(DateTime date, Timesheet timesheet)
        {
            if (timesheet == null)
                return $"<tr><td>{date.DayOfWeek}</td><td>{date.Day} {date.ToString("MMMM")}</td><td></td><td></td><td></td></tr>";

            return $"<tr><td>{date.DayOfWeek}</td><td>{date.Day} {date.ToString("MMMM")}</td><td>{timesheet.Bilable}</td><td>{timesheet.Km ?? 0}</td><td>{timesheet.Comments ?? ""}</td></tr>";
        }

        private static string ContractorTableRow(DateTime date, Timesheet timesheet)
        {
            if (timesheet == null)
                return $"<tr><td>{date.DayOfWeek}</td><td>{date.Day} {date.Month.ToString("MMMM")}</td><td></td><td></td><td></td><td></td><td></td><td></td><td></td></tr>";

            string start = $"<tr><td>{date.DayOfWeek}</td><td>{date.Day} {date.Month.ToString("MMMM")}</td><td>{timesheet.Bilable}</td>";
            string sick = $"<td><input class=\"form - check - input\" type=\"checkbox\" value=\"\" disabled {(timesheet.SickDay ? "ckecked" : "")}></td>";
            string holiday = $"<td><input class=\"form - check - input\" type=\"checkbox\" value=\"\" disabled {(timesheet.Holiday ? "ckecked" : "")}></td>";
            string timeoff = $"<td><input class=\"form - check - input\" type=\"checkbox\" value=\"\" disabled {(timesheet.TimeOff ? "ckecked" : "")}></td>";
            string leave = $"<td><input class=\"form - check - input\" type=\"checkbox\" value=\"\" disabled {(timesheet.SpecialLeave ? "ckecked" : "")}></td>";
            string end = $"<td>{timesheet.Km ?? 0}</td><td>{timesheet.Comments ?? ""}</td></tr>";
            return start + sick + holiday + timeoff + leave + end;
        }

    }
}
