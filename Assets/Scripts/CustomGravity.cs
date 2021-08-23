using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public static class CustomGravity
{
    static List<GravitySource> sources = new List<GravitySource>();

    public static Vector3 GetGravity(Vector3 _position) =>
            sources.Aggregate(Vector3.zero, (_current, _source) => _current + _source.GetGravity(_position));

    public static Vector3 GetUpAxis(Vector3 _position) =>
            -sources.Aggregate(Vector3.zero, (_current, _source) => _current + _source.GetGravity(_position)).normalized;

    public static Vector3 GetGravity(Vector3 _position, out Vector3 _upAxis)
    {
        Vector3 g = sources.Aggregate(Vector3.zero, (_current, _source) => _current + _source.GetGravity(_position));

        _upAxis = -g.normalized;

        return g;
    }

    public static void Register(GravitySource _source)
    {
        Debug.Assert(!sources.Contains(_source), "Duplicate registration of gravity source !", _source);
        
        sources.Add(_source);
    }

    public static void Unregister(GravitySource _source)
    {
        Debug.Assert(sources.Contains(_source), "Unregistration of unknown gravity source !", _source);
        
        sources.Remove(_source);
    }
}