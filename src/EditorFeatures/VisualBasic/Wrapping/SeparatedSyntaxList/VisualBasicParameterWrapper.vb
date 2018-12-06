﻿' Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

Imports Microsoft.CodeAnalysis.Text
Imports Microsoft.CodeAnalysis.VisualBasic.CodeGeneration
Imports Microsoft.CodeAnalysis.VisualBasic.Syntax

Namespace Microsoft.CodeAnalysis.VisualBasic.Editor.Wrapping.SeparatedSyntaxList
    Partial Friend Class VisualBasicParameterWrapper
        Inherits AbstractVisualBasicSeparatedSyntaxListWrapper(Of ParameterListSyntax, ParameterSyntax)

        Protected Overrides ReadOnly Property ListName As String = FeaturesResources.parameter_list
        Protected Overrides ReadOnly Property ItemNamePlural As String = FeaturesResources.parameters
        Protected Overrides ReadOnly Property ItemNameSingular As String = FeaturesResources.parameter

        Protected Overrides Function GetListItems(listSyntax As ParameterListSyntax) As SeparatedSyntaxList(Of ParameterSyntax)
            Return listSyntax.Parameters
        End Function

        Protected Overrides Function TryGetApplicableList(node As SyntaxNode) As ParameterListSyntax
            Return VisualBasicSyntaxGenerator.GetParameterList(node)
        End Function

        Protected Overrides Function PositionIsApplicable(
                root As SyntaxNode, position As Integer,
                declaration As SyntaxNode, listSyntax As ParameterListSyntax) As Boolean

            Dim generator = VisualBasicSyntaxGenerator.Instance
            Dim attributes = generator.GetAttributes(declaration)

            ' We want to offer this feature in the header of the member.  For now, we consider
            ' the header to be the part after the attributes, to the end of the parameter list.
            Dim firstToken = If(attributes?.Count > 0,
                attributes.Last().GetLastToken().GetNextToken(),
                declaration.GetFirstToken())

            Dim lastToken = listSyntax.GetLastToken()

            Dim headerSpan = TextSpan.FromBounds(firstToken.SpanStart, lastToken.Span.End)
            Return headerSpan.IntersectsWith(position)
        End Function
    End Class
End Namespace
