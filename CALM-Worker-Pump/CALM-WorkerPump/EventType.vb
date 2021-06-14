''' <summary>
''' Defines the type of worker event raised.
''' </summary>
''' <remarks></remarks>
Public Structure EventType
    Private _data As String
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
        If Not Me.isValid Then Return ""
        Return _data
    End Function
    ''' <summary>
    ''' Returns if this event is valid (Instanated and not blank)
    ''' </summary>
    ''' <value>A boolean</value>
    ''' <returns>Whether the event has been instanated and is not blank</returns>
    ''' <remarks></remarks>
    Public ReadOnly Property isValid As Boolean
        Get
            Return Not (_data Is Nothing OrElse _data = "")
        End Get
    End Property
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
    ''' <summary>
    ''' Checks if this object is equal to another.
    ''' </summary>
    ''' <param name="obj">The object to check with</param>
    ''' <returns>Whether this object is equal to another</returns>
    ''' <remarks></remarks>
    Public Overrides Function Equals(obj As Object) As Boolean
        If TypeOf obj Is EventType Then
            Return Me = CType(obj, EventType)
        End If
        Return False
    End Function
    ''' <summary>
    ''' Returns the hash code of the object
    ''' </summary>
    ''' <returns>The hash code</returns>
    ''' <remarks></remarks>
    Public Overrides Function GetHashCode() As Integer
        If _data Is Nothing Then Return 0
        Return _data.GetHashCode()
    End Function
End Structure
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