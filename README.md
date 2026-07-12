# FetchSpawner
The best PopUp Singleton Manager For Unity

Unity Version avariable: Tested on Unity 6, can be work other Unitys
How to install: Download the File <b>FetchSpawner.cs</b> and introduce in your proyect

# Uses

FetchSpawner its a reinvention of the Singleton, adapted to the PopUps

This Script makes esally the Code of repetitive systems who PopUps making more dynamic and esaly no spawn and destroy then

<b> example PopUpBox </b>
```

public class PopUpBox : FetchSpawner<PopUpBox, PopUpBox.Data>
{
    [SerializeField] private TextMeshProUGUI TextMeshPro;

    public class Data
    {
        public string Txt; // Text To view

        public Data(string Txt)
        {
            this.Txt = Txt;
        }
    }

    public override void OnFetch(Data s)
    {
        //All this code will be execute when the Fetch has spawned, Start Dont works correctly
        TextMeshPro.text = Txt;
    }

    // Odviusly you can use other functions of the Monobehaviour
    void Update
    {
        float s = Mathf.Sin(Time.time);
        TextMeshPro.color = new Color(s * 0.3, s * 0.3, s * 0.3)
    }

    public override GameObject GetMyPrefab()
    {
        return GameCore.Database.Fetches.PopUpFetch; // You can obtain the prefab using a database, of using Resources.Load()
    }
}

...

```

This script must be attached to the root GameObject of the prefab.

Once it's set up, it can be accessed from other scripts as follows:

```
PopUpBox.Fetch(new PopUpBox.Data("You obtained a sword!"));
```
→ The prefab is instantiated and displays the message "You obtained a sword!".

```
PopUpBox.Fetch(new PopUpBox.Data("Connection error"));
```
→ The prefab is instantiated and displays the message "Connection error".

```
PopUpBox.DestroyFetch();
```
→ Destroys the current Singleton instance.

```
PopUpBox.Instance;
```
→ Returns the current Singleton instance. You can also obtain the instance directly from the return value of Fetch(...).
