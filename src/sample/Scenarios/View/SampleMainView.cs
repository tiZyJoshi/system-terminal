using System;
using System.Globalization;
using System.Views;
using System.Views.Visuals;

namespace Sample.Scenarios.View
{
    sealed class SampleMainView : View<SampleApplication, SampleMainState>
    {
        public SampleMainView()
            : base(SampleApplication.Instance)
        {
        }

        protected override SampleMainState Initialize()
        {
            return new();
        }

        protected override SampleMainState Update(SampleMainState state, object message)
        {
            return message switch
            {
                "++" => new()
                {
                    Count = state.Count + 1,
                },
                "--" => new()
                {
                    Count = state.Count - 1,
                },
                _ => throw new InvalidOperationException(),
            };
        }

        protected override Page Render(SampleMainState state)
        {
            return new Page(new StackPanel
            {
                ["Count"] = new Label
                {
                    Text = state.Count.ToString(CultureInfo.CurrentCulture),
                },
                ["Increment"] = new Button
                {
                    Text = "Increment",
                    Pressed = () => Dispatch("++"),
                },
                ["Decrement"] = new Button
                {
                    Text = "Decrement",
                    Pressed = () => Dispatch("--"),
                },
            });
        }
    }
}
