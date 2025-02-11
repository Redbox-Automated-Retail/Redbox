using System;
using Redbox.HAL.Core;

namespace Redbox.HAL.Script.Framework
{
    [Serializable]
    public sealed class RuntimeInstruction : Instruction
    {
        public override string Mnemonic => "RUNTIME";

        public override bool IsCooperativeMultitask => false;

        protected override bool IsOperandRecognized(string operand)
        {
            return "YIELD-ONLY".Equals(operand);
        }

        protected override void ParseOperands(TokenList operandTokens, ExecutionResult result)
        {
            if (operandTokens[0].Type == TokenType.Symbol)
                return;
            result.AddInvalidOperandError("Expected YIELD-ONLY as an operand.");
        }

        protected override void OnExecute(ExecutionResult result, ExecutionContext context)
        {
            if (Operands.Count != 1)
            {
                AddError("E999", "The RUNTIME instruction expects one token", result.Errors);
            }
            else
            {
                if (!Operands[0].Value.Equals("YIELD-ONLY", StringComparison.CurrentCultureIgnoreCase))
                    return;
                ExecutionEngine.Instance.PerformContextSwitch(true);
            }
        }
    }
}