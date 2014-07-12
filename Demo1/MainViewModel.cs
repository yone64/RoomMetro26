using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Codeplex.Reactive;

namespace Demo1
{
    class MainViewModel
    {
        public ReactiveProperty<string> Input { get; private set; }
        public ReactiveProperty<string> Output { get; private set; }

        public MainViewModel()
        {
            this.Input = new ReactiveProperty<string>("");

            this.Output = Input.ToReactiveProperty();
        }
    }
}
