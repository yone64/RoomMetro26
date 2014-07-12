using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Codeplex.Reactive;

namespace Demo1
{
    class MainViewModel
    {
        private const string url = "http://www.google.com/complete/search?output=toolbar&q=";
        public ReactiveProperty<List<string>> Result { get; private set; }

        public ReactiveProperty<string> Input { get; private set; }
        public ReactiveProperty<string> Output { get; private set; }

        public MainViewModel()
        {
            this.Input = new ReactiveProperty<string>("");

            Output = Input
                .Throttle(TimeSpan.FromMilliseconds(300))
                .DistinctUntilChanged()
                .ToReactiveProperty();

            Result = Output
                .Where(s => !string.IsNullOrEmpty(s))
                .Select(s => new HttpClient().GetStringAsync(url + s).ToObservable())
                .Switch()
                .Select(x => XDocument.Parse(x))
                .Select(x => x.Descendants("suggestion").Select(e => (string)e.Attribute("data")).ToList())
                .Catch(Observable.Never<List<string>>())
                .ToReactiveProperty();
        }
    }
}


#region memo
        //private const string url = "http://www.google.com/complete/search?output=toolbar&q=";
        //public ReactiveProperty<List<string>> Result { get; private set; }

            //Output = Input
            //    .Throttle(TimeSpan.FromMilliseconds(300))
            //    .DistinctUntilChanged()
            //    .ToReactiveProperty();

            //Result = Output
            //    .Where(s => !string.IsNullOrEmpty(s))
            //    .Select(s => new HttpClient().GetStringAsync(url + s).ToObservable())
            //    .Switch()
            //    .Select(x => XDocument.Parse(x))
            //    .Select(x => x.Descendants("suggestion").Select(e => (string)e.Attribute("data")).ToList())
            //    .Catch(Observable.Never<List<string>>())
            //    .ToReactiveProperty();
#endregion