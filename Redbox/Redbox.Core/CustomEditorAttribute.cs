using System;

namespace Redbox.Core
{
    public class CustomEditorAttribute : Attribute
    {
        public CustomEditorAttribute(string text)
        {
            Text = text;
        }

        public string Text { get; private set; }

        public string GetMethodName { get; set; }

        public string SetMethodName { get; set; }
    }
}