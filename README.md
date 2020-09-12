# Compiladores2_Sintactico
LaboratorioA HectorDavila

Disculpe Ingneira lamentablemente no logre terminar el laboratorio, no le puedo decir otra coas que de fue error mio y que lo unico que puedo decir es que tendre que poner mayor esfuerzo en la siguiente paso del proyecto, lamento decpcionarla y el no haber cumplido con el trabjo de la amnera adecuada

Hector Davila 1115814

Gramatica Incompleta

program = DECL Program2

Program2 = DECL | e

Variable DECL = Variable2 ;

Variable2 = TYPE ident

TYPE = int TYPE2 | double TYPE2 | string TYPE2 | ident TYPE2

TYPE2 = [ ] TYPE2 | e

Function DECL = type ident (Formals) Func2 void ident ( Formals ) Func2 

Func2 = stmt | e

Formals = Variable Formals2 | e

Formals2 = Variable Formals2 | e

stmt = ifstmt | returnstmt | expr

ifstmt = if ( Expr ) stmt ( else stmt )

returnstmt = return ( Expr return2 ; )

return2 = expr return2 | e
