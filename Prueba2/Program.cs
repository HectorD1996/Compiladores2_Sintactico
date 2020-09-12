using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.IO;

namespace MINIC
{
    static class Program
    {
        
        static void Main(string[] args)
        {
            List<string> Tokens = new List<string>();
            List<string> definiciones = new List<string>();
            //Prueba
            Console.WriteLine("Please enter your text file path");
            String a = Console.ReadLine();
            string text = ""; 
            bool errorB = true;
            string sample = System.IO.File.ReadAllText(a.ToString());
            string[] output = a.Split('.');
            var defs = new TokenDefinition[]
            {
           
            //comentarios
            new TokenDefinition(@"(/\*([^*]|[\r\n]|(\*+([^*/]|[\r\n])))*\*+/)|^(//.*)", "T_Comentario"),
            new TokenDefinition(@"/\*.*", "Inicio_Coment"),
            new TokenDefinition(@".*\*/", "Final_Coment"),
            
            //string
            new TokenDefinition(@"([""'])(?:\\\1|.)*?\1", "T_string"),
            new TokenDefinition(@"^[\""].*" , "Incomplete_String"),
            //numeros
            new TokenDefinition(@"[-+]?\d+\.\d*([eE][-+]?\d+)?", "T_DoubleConstant"),
            new TokenDefinition(@"[0][Xx][\d*a-fA-F]+", "T_Hexadecimal"),
            new TokenDefinition(@"[-+]?\d+", "T_IntConstant"),
            //true y false
            new TokenDefinition(@"[t][r][u][e]", "T_TRUE"),
            new TokenDefinition(@"[f][a][l][s][e]", "T_FALSE"),
            //PALABRAS RESERVADAS
            new TokenDefinition(@"[v][o][i][d](?![_A-Za-z\d]+)", "T_void"),
            new TokenDefinition(@"[i][n][t](?![A-Za-z\d_]+)", "T_int"),
            new TokenDefinition(@"[d][o][u][b][l][e](?![A-Za-z\d_]+)", "T_double"),
            new TokenDefinition(@"[b][o][o][l](?![A-Za-z\d_]+)", "T_bool"),
            new TokenDefinition(@"[s][t][r][i][n][g](?![A-Za-z\d_]+)", "T_string"),
            new TokenDefinition(@"[c][l][a][s][s](?![A-Za-z\d_]+)", "T_class"),
            new TokenDefinition(@"[c][o][n][s][t](?![A-Za-z\d_]+)", "T_const"),
            new TokenDefinition(@"[i][n][t][e][r][f][a][c][e](?![A-Za-z\d_]+)", "T_interface"),
            new TokenDefinition(@"[n][u][l][l](?![A-Za-z\d_]+)", "T_null"),
            new TokenDefinition(@"[t][h][i][s](?![A-Za-z\d_]+)", "T_this"),
            new TokenDefinition(@"[f][o][r](?![A-Za-z\d_]+)", "T_for"),
            new TokenDefinition(@"[w][h][i][l][e](?![A-Za-z\d_]+)", "T_while"),
            new TokenDefinition(@"[f][o][r][e][a][c][h](?![A-Za-z\d_]+)", "T_foreach"),
            new TokenDefinition(@"[i][f](?![A-Za-z\d_]+)", "T_if"),
            new TokenDefinition(@"[e][l][s][e](?![A-Za-z\d_]+)", "T_else"),
            new TokenDefinition(@"[r][e][t][u][r][n](?![A-Za-z\d_]+)", "T_return"),
            new TokenDefinition(@"[b][r][e][a][k](?![A-Za-z\d_]+)", "T_break"),
            new TokenDefinition(@"[N][e][w](?![A-Za-z\d_]+)", "T_New"),
            new TokenDefinition(@"[N][e][w][A][r][r][a][y](?![A-Za-z\d_]+)", "T_NewArray"),
            new TokenDefinition(@"[C][o][n][s][o][l][e](?![A-Za-z\d_]+)", "T_Console"),
            new TokenDefinition(@"[W][r][i][t][e][L][i][n][e](?![A-Za-z\d_]+)", "T_WriteLine"),
            //identificadores
            new TokenDefinition(@"[_A-Za-z][_A-Za-z0-9]{0,30}", "T_Identifier"),
            //Operadores y carcteres de puntuacion
            new TokenDefinition(@"\.", "."),
            new TokenDefinition(@"\+", "+"),
            new TokenDefinition(@"\-", "-"),
            new TokenDefinition(@"\*", "*"),
            new TokenDefinition(@"\/", "/"),
            new TokenDefinition(@"\:", ":"),

            new TokenDefinition(@"[<][=]", ">"),
            new TokenDefinition(@"\<", "<"),

            new TokenDefinition(@"[>][=]", ">="),
            new TokenDefinition(@"\>", ">"),

            new TokenDefinition(@"[!][=]", "!="),
            new TokenDefinition(@"[=][=]", "=="),
            new TokenDefinition(@"\=", "="),
            new TokenDefinition(@"\!", "!"),


            new TokenDefinition(@"[&&]", "&&"),
            new TokenDefinition(@"[|][|]", "||"),


            new TokenDefinition(@"[[][]]", "[]"),
            new TokenDefinition(@"\[", "["),
            new TokenDefinition(@"\]", "]"),

            new TokenDefinition(@"[(][)]", "()"),
            new TokenDefinition(@"\(", "("),
            new TokenDefinition(@"\)", ")"),


            new TokenDefinition(@"\s", "SPACE"),
            new TokenDefinition(@"\%", "%"),

            new TokenDefinition(@"[{][}]", "{}"),
            new TokenDefinition(@"\}", "}"),
            new TokenDefinition(@"\{", "{"),

            new TokenDefinition(@"\,", ","),
            new TokenDefinition(@"\;", ";"),
            //error
            new TokenDefinition(@".", "Error")
            };
            TextReader r = new StringReader(sample);
            lexer l = new lexer(r, defs);
            while (l.Next())
            {
                if (l.Token.ToString() == "Inicio_Coment") { errorB = false; }
                if (l.Token.ToString() == "Final_Coment" & !errorB) { errorB = true; }
                    if (l.Token.ToString() != "Error" && l.TokenContents.ToString().Length < 31 && l.Token.ToString() != "Incomplete_String")
                    {
                        if (l.Token.ToString() != "SPACE" & l.Token.ToString() != "T_Comentario" & l.Token.ToString() != "Final_Coment" & errorB)
                        {
                            Tokens.Add(l.TokenContents);
                            definiciones.Add(l.Token.ToString());
                            text += l.TokenContents + " line " + l.LineNumber + " cols " + l.Position +  " is " + l.Token + "\n\r";
                            System.IO.File.WriteAllText(output[0] + ".out", text);
                            
                        }
                    }
                    else if(l.TokenContents.ToString().Length < 31 && l.Token.ToString() != "Incomplete_String")
                    {
                        text += "*** Error line "+ l.LineNumber + " *** Unrecognized char: " + l.TokenContents + "\n\r";
                        System.IO.File.WriteAllText(output[0] + ".out", text);
                        

                    }
                    else if(l.Token.ToString() != "T_Comentario" && l.Token.ToString() != "Incomplete_String")
                        {
                            text += "*** Error line " + l.LineNumber + " Secuencia mayor al limite permitido" + "\n\r";
                            System.IO.File.WriteAllText(output[0] + ".out", text);
                            

                        }
                        else if(l.Token.ToString() == "Incomplete_String")
                        {
                            text += "*** Error line " + l.LineNumber + " string sin terminar" + "\n\r";
                            System.IO.File.WriteAllText(output[0] + ".out", text);
                            
                        }
               
                
            }
            if (!errorB)
            {
                text += "*** Error line "+ l.LineNumber +  "*** EOF in unfinished comment: " + "\n\r";
                System.IO.File.WriteAllText(output[0] + ".out", text);
                
            }
            else if (l.Token.ToString() == "Incomplete_String")
            {
                text += "*** Error line " + l.LineNumber + "*** EOF in unfinished string: " + "\n\r";
                System.IO.File.WriteAllText(output[0]+".out", text);
                
            }
            Console.WriteLine(text);
            Sintatico S = new Sintatico(Tokens, definiciones);

            Console.ReadKey();
        }

        public interface IMatcher
        { 
            int Match(string text);
        }

        sealed class RegexMatcher : IMatcher
        {
            private readonly Regex regex;
            public RegexMatcher(string regex)
            {
                this.regex = new Regex(string.Format("^{0}", regex));
            }
            public int Match(string text)
            {
                var m = regex.Match(text); return m.Success ? m.Length : 0;
            }

            public override string ToString()
            {
                return regex.ToString();
            }
        }

        public sealed class TokenDefinition
        {
            public readonly IMatcher Matcher;
            public readonly object Token;
            public TokenDefinition(string regex, object token)
            {
                this.Matcher = new RegexMatcher(regex);
                this.Token = token;
            }
        }

        public sealed class lexer : IDisposable
        {
            private readonly TextReader reader;
            private readonly TokenDefinition[] tokenDefinitions;
            private string lineRemaining;
            public lexer(TextReader reader, TokenDefinition[] tokenDefinitions)
            {
                this.reader = reader;
                this.tokenDefinitions = tokenDefinitions;
                nextLine();
            }
            private void nextLine()
            {
                do
                {
                    lineRemaining = reader.ReadLine();
                    ++LineNumber;
                    Position = 0;
                }
                while (lineRemaining != null && lineRemaining.Length == 0);
            }

            public bool Next()
            {
               
                if (lineRemaining == null) return false;
                foreach (var def in tokenDefinitions)
                {
                    var matched = def.Matcher.Match(lineRemaining);
                    if (matched > 0)
                    {
                        Position += matched; Token = def.Token;
                        TokenContents = lineRemaining.Substring(0, matched);
                        lineRemaining = lineRemaining.Substring(matched);
                        if (lineRemaining.Length == 0)
                            nextLine(); return true;
                    }
                }

                throw new Exception(string.Format("Unable to match against any tokens at line {0} position {1} \"{2}\"", LineNumber, Position, lineRemaining));
            }

            public string TokenContents { get; private set; }

            public object Token { get; private set; }

            public int LineNumber { get; private set; }

            public int Position { get; private set; }

            //public bool ErrorRG { get; private set; }

            public void Dispose() { reader.Dispose(); }
        }

        public class Sintatico
        {
            int tokenactual = 0;
            List<string> TokensS = new List<string>();
            List<string> DefinicionesS = new List<string>();
            public Sintatico(List<string> Tokens1, List<string> definiciones1)
            {
                TokensS = Tokens1;
                DefinicionesS = definiciones1;
            }
            public bool DECL(string Tokens1, string definiciones1)
            {
                
                if (VariableDecl(TokensS[tokenactual], DefinicionesS[tokenactual]))
                {
                    return true;
                }
                else if(FunctionDecl(TokensS[tokenactual], DefinicionesS[tokenactual]))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

            public bool VariableDecl(string TokenA, string definicionA)
            {
                if (Variable(TokensS[tokenactual], DefinicionesS[tokenactual]))
                {
                    tokenactual++;
                    if (TokenA == ";")
                    {
                        tokenactual++;
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else 
                {
                    tokenactual--;
                    return false;
                }
            }

            public bool Variable(string TokenA, string definicionA)
            {
                if (Type(TokensS[tokenactual], DefinicionesS[tokenactual]))
                {
                    if (DefinicionesS[tokenactual] == "T_Identifier")
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }

            }

            public bool Type(string TokenA, string definicionA)
            {
                switch (definicionA)
                {
                    case "T_int":
                        tokenactual++;
                        return true;
                        
                    case "T_double":
                        tokenactual++;
                        return true;
                    case "T_bool":
                        tokenactual++;
                        return true;
                    case "T_string":
                        tokenactual++;
                        return true;
                    case "T_Identifier":
                        tokenactual++;
                        return true;

                    default:
                        return false;
                        break;
                }
            }

            public bool FunctionDecl(string TokenA, string definicionA)
            {
                int inicioToken = tokenactual;
                if (Type(TokensS[tokenactual], DefinicionesS[tokenactual]))
                {
                    tokenactual++;
                    if (DefinicionesS[tokenactual] == "T_identifier")
                    {
                        tokenactual++;
                        if (DefinicionesS[tokenactual] == "(")
                        {
                            tokenactual++;
                            if (Formals(TokensS[tokenactual], DefinicionesS[tokenactual]))
                            {
                                tokenactual++;
                                if (FunctionDecl2(TokensS[tokenactual], DefinicionesS[tokenactual]))
                                {
                                    tokenactual++;
                                    if (DefinicionesS[tokenactual] == "T_void")
                                    {
                                        tokenactual++;
                                        if (DefinicionesS[tokenactual] == "T_identifier")
                                        {
                                            tokenactual++;
                                            if (DefinicionesS[tokenactual] == "(")
                                            {
                                                if (Formals(TokensS[tokenactual], DefinicionesS[tokenactual]))
                                                {
                                                    if (FunctionDecl2(TokensS[tokenactual], DefinicionesS[tokenactual]))
                                                    {
                                                        return true;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                tokenactual = inicioToken;
                return false;
            }

            public bool FunctionDecl2(string TokenA, string definicionA)
            {
                if (Stmt(TokensS[tokenactual], DefinicionesS[tokenactual]))
                {
                    if (FunctionDecl2(TokensS[tokenactual], DefinicionesS[tokenactual]))
                    {
                        return true;
                    }
                    else
                    {
                        return true;
                    }
                }
                else
                {
                    return true;
                }
            }

            public bool Formals(string TokenA, string definicionA)
            {
                if (Variable(TokensS[tokenactual], DefinicionesS[tokenactual]))
                {
                    if (Formals2(TokensS[tokenactual], DefinicionesS[tokenactual]))
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else { return false; }
            }

            public bool Formals2(string TokenA, string definicionA)
            {
                if (Variable(TokensS[tokenactual], DefinicionesS[tokenactual]))
                {
                    if (Formals2(TokensS[tokenactual], DefinicionesS[tokenactual]))
                    {
                        return true;
                    }
                    else
                    {
                        tokenactual--;
                        return true;

                    }


                }
                else
                {
                    tokenactual--;
                    return true;
                }
            }

            public bool Stmt(string TokenA, string definicionA)
            {
                if (ifStmt(TokensS[tokenactual], DefinicionesS[tokenactual]))
                {
                    
                    return true;
                }else
                if (ifStmt(TokensS[tokenactual], DefinicionesS[tokenactual]))
                {

                    return true;
                }else
                if (ifStmt(TokensS[tokenactual], DefinicionesS[tokenactual]))
                {

                    return true;
                }
                else
                {
                    return false;
                }
            }
            public bool ifStmt(string TokenA, string definicionA)
            {
                if (definicionA == "T_if")
                {
                    tokenactual++;
                    if (Stmt(TokensS[tokenactual], DefinicionesS[tokenactual]))
                    {
                        if (definicionA == "(")
                        {
                            tokenactual++;
                            if (Expr())
                            {
                                if (definicionA == ")")
                                {
                                    if (Stmt(TokensS[tokenactual], DefinicionesS[tokenactual]))
                                    {
                                        if (definicionA == "(")
                                        {
                                            tokenactual++;
                                            if (Expr())
                                            {
                                               
                                                    tokenactual++;
                                                    if (DefinicionesS[tokenactual] == "T_else")
                                                    {

                                                        if (Stmt(TokensS[tokenactual], DefinicionesS[tokenactual]))
                                                        {
                                                        if (definicionA == ")")
                                                        { return true; }
                                                        }
                                                    }
                                                
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                return false;
            }
            public bool returnStmt(string TokenA, string definicionA)
            {
                if (definicionA=="T_return")
                {
                    tokenactual++;
                    if (DefinicionesS[tokenactual] == "(")
                    {
                        tokenactual++;
                        if (Expr())
                        {
                            if (returnStmt2())
                            {
                                if (DefinicionesS[tokenactual] == ",")
                                {
                                    tokenactual++;
                                    if (definicionA == ")")
                                    {
                                        tokenactual++;
                                        if (definicionA == ";")
                                        {
                                            return true;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                return false;
            }
            public bool returnStmt2()
            {
                if (Expr())
                {
                    if (returnStmt2())
                    {
                        return true;
                    }
                    return true;
                }
                else
                {
                    return true;
                }
            }
            public bool Expr()
            {
                tokenactual++;
                return true;
            }

        }
    }


}

