using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchHandlerBJH : MonoBehaviour
{
    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Vector2 touchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(touchPosition, Vector2.zero);

            if(hit.collider != null && hit.collider.CompareTag("BallPosition"))
            {
                SpriteRenderer renderer = hit.collider.gameObject.GetComponent<SpriteRenderer>();
                if(renderer.sprite != null)
                {
                    string breadName = renderer.sprite.name;
                    RecipeGameManager.Instance.selectedIngredientDic.Remove(breadName);
                    renderer.sprite = null;
                }
            }

        }
    }
}
