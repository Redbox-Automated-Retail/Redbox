using System;

namespace Redbox.Core
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class RecurseAttribute : Attribute
    {
    }
}