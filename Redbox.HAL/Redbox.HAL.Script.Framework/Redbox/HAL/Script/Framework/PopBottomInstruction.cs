using System;
using Redbox.HAL.Core;

namespace Redbox.HAL.Script.Framework
{
    [Serializable]
    public sealed class PopBottomInstruction : Instruction
    {
        public override string Mnemonic => "POPB";

        protected override void ParseOperands(TokenList operandTokens, ExecutionResult result)
        {
            if (operandTokens.Count == 0 || operandTokens.Count > 2)
            {
                result.AddInvalidOperandError(
                    "Expected one symbol to receive top of stack, with an optional DEPTH=n key value pair.");
            }
            else
            {
                if (operandTokens[0].Type == TokenType.Symbol)
                    return;
                result.AddError(ExecutionErrors.InvalidOperand,
                    "Expected no operand or a symbol operand, with an optional DEPTH=n key value pair.");
            }
        }

        protected override void OnExecute(ExecutionResult result, ExecutionContext context)
        {
            var obj = (object)null;
            var nullable1 = GetKeyValuePairValue<int?>(Operands.GetKeyValuePair("DEPTH"), result.Errors, context);
            if (result.Errors.Count > 0)
                return;
            if (!nullable1.HasValue)
                nullable1 = 1;
            var stackDepth = context.StackDepth;
            var nullable2 = nullable1;
            var valueOrDefault1 = nullable2.GetValueOrDefault();
            if ((stackDepth >= valueOrDefault1) & nullable2.HasValue)
            {
                var num1 = 0;
                while (true)
                {
                    var num2 = num1;
                    var nullable3 = nullable1;
                    var valueOrDefault2 = nullable3.GetValueOrDefault();
                    if ((num2 < valueOrDefault2) & nullable3.HasValue)
                    {
                        obj = context.PopBottom();
                        ++num1;
                    }
                    else
                    {
                        break;
                    }
                }

                var operand = Operands[0];
                if (operand.Type != TokenType.Symbol)
                    return;
                context.SetSymbolValue(operand.Value, obj);
            }
            else
            {
                AddError("E002",
                    string.Format("The specified DEPTH of {0} exceeds the current depth of the stack: {1}.", nullable1,
                        context.StackDepth), result.Errors);
            }
        }
    }
}