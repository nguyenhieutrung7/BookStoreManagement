using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace Model.Infrastructure
{
    public class LanguageManagement
    {
        public static List<Language> AvailableLanguages = new List<Language>() {
            new Language{ LanguageFullName="Vietnamese",LanguageCultureName="vi"},
            new Language{ LanguageFullName="English",LanguageCultureName="en"}
            
        };

        //check language is exist in available languages
        public static bool IsLanguageAvailable(string language)
        {
            return AvailableLanguages.Where(a => a.LanguageCultureName.Equals(language)).FirstOrDefault() != null ? true : false;
        }

        //get default language
        public static string GetDefaultLanguage()
        {
            return AvailableLanguages.ElementAt(0).LanguageCultureName;
        }

        public static void SetCurrentLanguage(string language)
        {
            try
            {
                if (!IsLanguageAvailable(language))
                {
                    language = GetDefaultLanguage();
                }
                var cultureInfo = new CultureInfo(language);
                Thread.CurrentThread.CurrentUICulture = cultureInfo;
                Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(cultureInfo.Name);
                HttpCookie languageCookie = new HttpCookie("culture", language);
                languageCookie.Expires = DateTime.Now.AddYears(1);
                HttpContext.Current.Response.Cookies.Add(languageCookie);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
