using Microsoft.Xna.Framework;

namespace Project_B_Client_App.Payloads;

public record ReceivePositionPayload(string User, float X, float Y, float RotationRadians, Vector2 Direction) { }