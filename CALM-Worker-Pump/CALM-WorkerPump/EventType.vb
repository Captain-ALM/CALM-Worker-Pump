''' <summary>
''' Defines the type of worker event raised.
''' </summary>
''' <remarks></remarks>
Public Class EventType
    Private _data As String = ""
    ''' <summary>
    ''' Initalises the none EventType.
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub New()
        _data = "none"
    End Sub
    ''' <summary>
    ''' Initalises the passed data as a lowercase string.
    ''' </summary>
    ''' <param name="data">The EventType</param>
    ''' <remarks></remarks>
    Public Sub New(data As String)
        _data = data.ToLower
    End Sub
    ''' <summary>
    ''' Returns the EventType (Lowercase String Name of the Event)
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function getEvent() As String
        Return _data
    End Function
    Shared Widening Operator CType(str As String) As EventType
        Return New EventType(str)
    End Operator
    Shared Widening Operator CType(et As EventType) As String
        Return et.getEvent()
    End Operator
    Shared Operator =(v1 As EventType, v2 As EventType) As Boolean
        Return v1.getEvent() = v2.getEvent()
    End Operator
    Shared Operator <>(v1 As EventType, v2 As EventType) As Boolean
        Return v1.getEvent() <> v2.getEvent()
    End Operator
End Class
''' <summary>
''' Provides some pre-defined EventTypes.
''' </summary>
''' <remarks></remarks>
Public Class EventTypes
    Public Shared ReadOnly None As New EventType("None")
    Public Shared ReadOnly Click As New EventType("Click")
    Public Shared ReadOnly Load As New EventType("Load")
    Public Shared ReadOnly Shown As New EventType("Shown")
    Public Shared ReadOnly Closing As New EventType("Closing")
    Public Shared ReadOnly Closed As New EventType("Closed")
    Public Shared ReadOnly KeyDown As New EventType("KeyDown")
    Public Shared ReadOnly KeyUp As New EventType("KeyUp")
    Public Shared ReadOnly KeyPress As New EventType("KeyPress")
End Class