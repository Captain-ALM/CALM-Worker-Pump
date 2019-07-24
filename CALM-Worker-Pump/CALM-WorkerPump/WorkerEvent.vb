''' <summary>
''' This contains information about a worker event.
''' </summary>
''' <remarks></remarks>
Public Class WorkerEvent
    Inherits EventArgs
    ''' <summary>
    ''' Defines the event source.
    ''' </summary>
    ''' <remarks></remarks>
    Public EventSource As Source
    ''' <summary>
    ''' Defines the event type.
    ''' </summary>
    ''' <remarks></remarks>
    Public EventType As EventType
    ''' <summary>
    ''' Defines the event argument.
    ''' </summary>
    ''' <remarks></remarks>
    Public EventData As EventArgs
    ''' <summary>
    ''' Creates a new worker event.
    ''' </summary>
    ''' <param name="so">Source Object</param>
    ''' <param name="et">Event Type</param>
    ''' <param name="ed">Event Data</param>
    ''' <remarks></remarks>
    Public Sub New(so As Object, et As EventType, ed As EventArgs)
        EventSource = New Source(so)
        EventType = et
        EventData = ed
    End Sub
    ''' <summary>
    ''' Creates a new worker event.
    ''' </summary>
    ''' <param name="so">Source Object</param>
    ''' <param name="sop">Source Object Parent List</param>
    ''' <param name="et">Event Type</param>
    ''' <param name="ed">Event Data</param>
    ''' <remarks></remarks>
    Public Sub New(so As Object, sop As List(Of Object), et As EventType, ed As EventArgs)
        EventSource = New Source(so, sop)
        EventType = et
        EventData = ed
    End Sub
    ''' <summary>
    ''' Defines a source object.
    ''' </summary>
    ''' <remarks></remarks>
    Public Structure Source
        ''' <summary>
        ''' Defines the ultimate child source object.
        ''' </summary>
        ''' <remarks></remarks>
        Public sourceObj As Object
        ''' <summary>
        ''' Defines all the parent objects.
        ''' </summary>
        ''' <remarks></remarks>
        Public parentObjs As List(Of Object)
        ''' <summary>
        ''' Creates a new worker event.
        ''' </summary>
        ''' <param name="so">Source Object</param>
        ''' <param name="pos">Source Object Parent List</param>
        ''' <remarks></remarks>
        Sub New(so As Object, Optional pos As List(Of Object) = Nothing)
            sourceObj = so
            parentObjs = pos
            If parentObjs Is Nothing Then
                parentObjs = New List(Of Object)
            End If
        End Sub
    End Structure
End Class