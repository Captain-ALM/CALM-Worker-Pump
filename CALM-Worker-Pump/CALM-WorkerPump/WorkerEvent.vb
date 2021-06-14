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
    ''' Defines the event replace mode.
    ''' </summary>
    ''' <remarks></remarks>
    Public EventReplaceMode As ReplaceMode
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
    ''' <param name="sop">Source Object Parent Array</param>
    ''' <param name="et">Event Type</param>
    ''' <param name="ed">Event Data</param>
    ''' <remarks></remarks>
    Public Sub New(so As Object, sop As Object(), et As EventType, ed As EventArgs)
        EventSource = New Source(so, sop)
        EventType = et
        EventData = ed
    End Sub
    ''' <summary>
    ''' Checks if this object is equal to another.
    ''' </summary>
    ''' <param name="obj">The object to check with</param>
    ''' <returns>Whether this object is equal to another</returns>
    ''' <remarks></remarks>
    Public Overrides Function Equals(obj As Object) As Boolean
        Dim toret As Boolean = False
        If TypeOf obj Is WorkerEvent Then
            toret = Me.EventSource.Equals(CType(obj, WorkerEvent).EventSource)
            toret = toret And Me.EventType.Equals(CType(obj, WorkerEvent).EventType)
        End If
        Return toret
    End Function

    Public Shared Operator =(o1 As WorkerEvent, o2 As WorkerEvent) As Boolean
        If Object.ReferenceEquals(o1, o2) Then
            Return True
        Else
            If Not o1 Is Nothing Then
                Return o1.Equals(o2)
            ElseIf Not o2 Is Nothing Then
                Return o2.Equals(o1)
            End If
        End If
        Return False
    End Operator

    Public Shared Operator <>(o1 As WorkerEvent, o2 As WorkerEvent) As Boolean
        Return Not (o1 = o2)
    End Operator

    ''' <summary>
    ''' Returns the hash code of the object
    ''' </summary>
    ''' <returns>The hash code</returns>
    ''' <remarks></remarks>
    Public Overrides Function GetHashCode() As Integer
        Return 0
        'I don't care dictionary about your buckets
    End Function

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
        Public parentObjs As Object()
        ''' <summary>
        ''' Creates a new worker event.
        ''' </summary>
        ''' <param name="so">Source Object</param>
        ''' <param name="pos">Source Object Parent Array</param>
        ''' <remarks></remarks>
        Sub New(so As Object, Optional pos As Object() = Nothing)
            sourceObj = so
            parentObjs = pos
        End Sub
        ''' <summary>
        ''' Checks if this object is equal to another.
        ''' </summary>
        ''' <param name="obj">The object to check with</param>
        ''' <returns>Whether this object is equal to another</returns>
        ''' <remarks></remarks>
        Public Overrides Function Equals(obj As Object) As Boolean
            Dim toret As Boolean = False
            If TypeOf obj Is Source Then
                If Object.ReferenceEquals(Me.sourceObj, CType(obj, Source).sourceObj) Then toret = True
                If (Not toret) And (Not Object.ReferenceEquals(Me.sourceObj, Nothing)) Then toret = True
                If toret AndAlso Me.sourceObj.Equals(CType(obj, Source).sourceObj) Then toret = True Else toret = False
                If toret And Not emptyOArrEq(CType(obj, Source).parentObjs, Me.parentObjs) Then
                    If Not lenOArrEq(CType(obj, Source).parentObjs, Me.parentObjs) Then
                        toret = False
                    Else
                        For i As Integer = 0 To Me.parentObjs.Length - 1 Step 1
                            If Not Object.ReferenceEquals(CType(obj, Source).parentObjs(i), Me.parentObjs(i)) Then
                                If Not Object.ReferenceEquals(Me.parentObjs(i), Nothing) Then
                                    If Not Me.parentObjs(i).Equals(CType(obj, Source).parentObjs(i)) Then
                                        toret = False
                                        Exit For
                                    End If
                                Else
                                    toret = False
                                    Exit For
                                End If
                            End If
                        Next
                    End If
                End If
            End If
            Return toret
        End Function

        ''' <summary>
        ''' Returns the hash code of the object
        ''' </summary>
        ''' <returns>The hash code</returns>
        ''' <remarks></remarks>
        Public Overrides Function GetHashCode() As Integer
            Return 0
            'I don't care dictionary about your buckets
        End Function

        Public Shared Operator =(o1 As Source, o2 As Source) As Boolean
            Return o1.Equals(o2)
        End Operator

        Public Shared Operator <>(o1 As Source, o2 As Source) As Boolean
            Return Not (o1 = o2)
        End Operator

        Private Function emptyOArrEq(obj1 As Object(), obj2 As Object()) As Boolean
            If obj1 Is Nothing And obj2 Is Nothing Then
                Return True
            ElseIf obj1 Is Nothing And Not (obj2 Is Nothing) Then
                Return obj2.Length = 0
            ElseIf obj2 Is Nothing And Not (obj1 Is Nothing) Then
                Return obj1.Length = 0
            End If
            Return False
        End Function

        Private Function lenOArrEq(obj1 As Object(), obj2 As Object()) As Boolean
            If obj1 Is Nothing And obj2 Is Nothing Then
                Return True
            ElseIf obj1 Is Nothing And Not (obj2 Is Nothing) Then
                Return obj2.Length = 0
            ElseIf obj2 Is Nothing And Not (obj1 Is Nothing) Then
                Return obj1.Length = 0
            Else
                Return obj1.Length = obj2.Length
            End If
        End Function
    End Structure
End Class
''' <summary>
''' Defines the way WorkerEvent classes are queued to the WorkerPump.
''' </summary>
''' <remarks></remarks>
Public Enum ReplaceMode As Integer
    ''' <summary>
    ''' Replaces the last event of the same type, source and parent objects if it exists in the pump already
    ''' </summary>
    ''' <remarks></remarks>
    ReplaceExisting = 0
    ''' <summary>
    ''' Does not replace the event of the same type, source and parent objects if it exists in the pump already
    ''' </summary>
    ''' <remarks></remarks>
    KeepExisting = 1
    ''' <summary>
    ''' Enqueues the event regardless of if an event of the same type, source and parent objects exists
    ''' </summary>
    ''' <remarks></remarks>
    Queue = 2
End Enum