using System;

namespace Redbox.HAL.Script.Framework
{
    [Serializable]
    public sealed class ExitInstruction : Instruction
    {
        public override string Mnemonic => "EXIT";

        public override bool ExitsContext => true;

        protected override ExpectedOperands Expected => ExpectedOperands.Optional;

        protected override void OnExecute(ExecutionResult result, ExecutionContext context)
        {
            if (Operands.Count > 0)
            {
                var symbolValue = Operands[0].Value;
                if (Operands[0].IsSymbolOrConst)
                {
                    symbolValue = (string)context.GetSymbolValue(Operands[0].Value, result.Errors);
                    if (symbolValue == null || result.Errors.Count > 0)
                    {
                        AddError("E999", "Unspecified context exit; check the logs.", result.Errors);
                        return;
                    }
                }

                AddError("E999", symbolValue, result.Errors);
            }
            else
            {
                AddError("E999", "Unspecified context exit; check the logs.", result.Errors);
            }
        }
    }
}