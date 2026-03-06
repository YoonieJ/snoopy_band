// SceneNode.cs
// A simple scene graph node for 2D sprites, supporting hierarchical transformations.
// Each node can have a texture, position, rotation, scale, and children nodes.
// Transformations are applied hierarchically, meaning a child node's transformation is relative to its parent.

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace snoopy_band;

public class SceneNode
{
    // The texture to be drawn for this node. If null, the node is used as an anchor or grouping node.
    public Texture2D Texture { get; set; }

    // The color tint applied to the texture. Defaults to white (no tint).
    public Color Tint { get; set; } = Color.White;

    public float Layer { get; set; } = 0f;

    // Local transformation properties
    public Vector2 LocalPosition { get; set; } // Position relative to the parent node.
    public float LocalRotation { get; set; } // Rotation in radians, relative to the parent node.
    public Vector2 LocalScale { get; set; } = Vector2.One; // Scale relative to the parent node.

    // The pivot point for rotation and scaling, specified in texture space.
    // For example, (0,0) is the top-left corner, and (width/2, height/2) is the center of the texture.
    public Vector2 Origin { get; set; } = Vector2.Zero;

    // The local transformation matrix, computed from position, rotation, and scale.
    public Matrix LocalMatrix { get; private set; } = Matrix.Identity;

    // The world transformation matrix, which combines the local matrix with the parent's world matrix.
    public Matrix WorldMatrix { get; private set; } = Matrix.Identity;

    // A list of child nodes. These nodes inherit transformations from this node.
    private readonly List<SceneNode> _children = new();

    // Constructor to initialize the node, optionally with a texture.
    public SceneNode(Texture2D texture = null)
    {
        Texture = texture;
    }

    // Adds a child node to this node. The child will inherit transformations from this node.
    public void AddChild(SceneNode child)
    {
        // keep children sorted by Layer (ascending)
        var i = 0;
        while (i < _children.Count && _children[i].Layer <= child.Layer)
            i++;

        _children.Insert(i, child);
    }

    // Provides read-only access to the list of child nodes.
    public IReadOnlyList<SceneNode> Children => _children;

    // Recomputes the local transformation matrix based on the current position, rotation, and scale.
    public void RecomputeLocal()
    {
        LocalMatrix =
            Matrix.CreateScale(LocalScale.X, LocalScale.Y, 1f) * // Apply scaling
            Matrix.CreateRotationZ(LocalRotation) * // Apply rotation
            Matrix.CreateTranslation(LocalPosition.X, LocalPosition.Y, 0f); // Apply translation
    }

    // Recomputes the world transformation matrix by combining the local matrix with the parent's world matrix.
    // This propagates transformations down the hierarchy.
    public void RecomputeWorld(Matrix parentWorld)
    {
        WorldMatrix = LocalMatrix * parentWorld; // Combine local and parent transformations
        foreach (var c in _children) // Recompute world matrices for all children
            c.RecomputeWorld(WorldMatrix);
    }

    // Inserts a child node at the specified index in the children list. 
    // If the index is out of bounds, it will be clamped to valid range.
    public void InsertChild(int index, SceneNode child)
    {
        if (index < 0) index = 0;
        if (index > _children.Count) index = _children.Count;
        _children.Insert(index, child);
    }

    // Draws the node's texture using the SpriteBatch, applying the world transformation.
    public void Draw(SpriteBatch sb)
    {
        if (Texture != null) // Only draw if the node has a texture
        {
            // Extract position, rotation, and scale from the world matrix
            var pos = new Vector2(WorldMatrix.M41, WorldMatrix.M42); // Translation (position)
            var rot = (float)Math.Atan2(WorldMatrix.M21, WorldMatrix.M11); // Rotation angle
            var sx = new Vector2(WorldMatrix.M11, WorldMatrix.M12).Length(); // Scale X
            var sy = new Vector2(WorldMatrix.M21, WorldMatrix.M22).Length(); // Scale Y

            sb.Draw(Texture, pos, null, Tint, rot, Origin, new Vector2(sx, sy), SpriteEffects.None, 0f);
        }

        // Recursively draw all child nodes
        foreach (var c in _children)
            c.Draw(sb);
    }
}