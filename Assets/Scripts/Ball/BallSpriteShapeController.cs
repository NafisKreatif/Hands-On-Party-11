using UnityEngine;
using UnityEngine.U2D;

class BallSpriteShapeController : MonoBehaviour {
    private SpriteShapeController _spriteShapeController;
    private void Start() {
        _spriteShapeController = GetComponent<SpriteShapeController>();
    }

    public void SetSpriteShape(SpriteShape spriteShape) {
        _spriteShapeController.spriteShape = spriteShape;
    }
}