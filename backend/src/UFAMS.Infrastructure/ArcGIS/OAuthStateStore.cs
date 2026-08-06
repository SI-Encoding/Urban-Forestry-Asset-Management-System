using System.Collections.Concurrent;

namespace UFAMS.Infrastructure.ArcGIS;

public class OAuthStateStore
{
    private readonly ConcurrentDictionary<string, OAuthState> _states = new();

    public void Save(OAuthState state)
    {
        _states[state.State] = state;
    }

    public OAuthState? Take(string state)
    {
        if (_states.TryRemove(state, out var value))
        {
            if (value.ExpiresAt > DateTime.UtcNow)
                return value;
        }

        return null;
    }
}