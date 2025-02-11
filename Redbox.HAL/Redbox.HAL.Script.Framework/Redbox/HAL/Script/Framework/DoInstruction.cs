using System;
using Redbox.HAL.Core;

namespace Redbox.HAL.Script.Framework
{
    [Serializable]
    public sealed class DoInstruction : Instruction
    {
        public override bool BeginsContext => true;

        public override string EndContextMnemonic => "LOOP";

        public override string Mnemonic => "DO";

        internal override bool ValidateConditionalBranch(Instruction target, ExecutionResult result)
        {
            if (!target.Mnemonic.Equals(EndContextMnemonic))
            {
                LogIncorrectBranch(target.Mnemonic, result);
                return false;
            }

            CounterSymbol = Mnemonic + LineNumber;
            target.FalseBranchTarget = TrueBranchTarget;
            target.CounterSymbol = CounterSymbol;
            return true;
        }

        protected override void ParseOperands(TokenList operandTokens, ExecutionResult result)
        {
            if (operandTokens.Count != 1)
            {
                result.AddError(ExecutionErrors.InvalidOperand,
                    "The DO mnemonic requires a single numeric literal or symbol.");
            }
            else
            {
                if (operandTokens[0].Type == TokenType.NumericLiteral || operandTokens[0].IsSymbolOrConst)
                    return;
                result.AddError(ExecutionErrors.InvalidOperand, "Expected a numeric literal or (CONST) symbol.");
            }
        }

        protected override void OnExecute(ExecutionResult result, ExecutionContext context)
        {
            var symbolValue = (object)Operands[0].Value;
            if (Operands[0].Type == TokenType.Symbol)
            {
                symbolValue = context.GetSymbolValue(Operands[0].Value, result.Errors);
                if (symbolValue == null || result.Errors.Count > 0)
                    return;
            }

            var int32 = Convert.ToInt32(symbolValue);
            if (int32 <= 0)
                return;
            X = int32;
            result.SwitchToContext = this;
        }
    }
}