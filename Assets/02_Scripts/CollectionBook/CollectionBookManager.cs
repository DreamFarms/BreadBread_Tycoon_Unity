using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectionBookManager : MonoBehaviour
{
    public List<CollectionCustomer> customers;
    public List<Sprite> customerSprite;
    public Sprite unKnownSprite;
    public GameObject popUp;
    public CollectionBookConnection connection;

    private void Awake()
    {
        connection.CollectionRequest();
    }

    public void AddCustomer(CollectionResponseMessage response)
    {

        for (int i = 0; i < customers.Count; i++)
        {
            if (customers[i].customerName == string.Empty)
            {
                 Sprite customerImage = LoadCustomerImage(response.name);
                Debug.Log(customerImage.name);
                customers[i].SetCustomerInfo(
                    response.name,
                    response.intro,
                    response.preferredBreads,
                    response.preferredBreadsVisible,
                    response.visitCount,
                    response.unlocked,
                    customerImage
                );
                return ;
            }
        }
    }

    public Sprite LoadCustomerImage(string name)
    {
        for (int i = 0; i < customers.Count; i++)
        {
            if (customerSprite[i].name == name)
            {
                return customerSprite[i];
            }
        }
        return unKnownSprite;
    }
}
