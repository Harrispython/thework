using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using echo17.EndlessBook;

[System.Serializable]
public class BookAnimator : MonoBehaviour
{
    [SerializeField]
    private Animator _animator;
    [SerializeField]
    private Image _character;
    [SerializeField]
    private TextMeshProUGUI _characterName;
    [SerializeField]
    private List<Sprite> _characterList;
    [SerializeField]
    private List<string> _nameList;
    [SerializeField]
    private EndlessBook _book;
    [SerializeField]
    private BookEnum _bookEnum;
    public TextMeshProUGUI Description;
    [TextArea(5, 10)]
    public string Text;
    public Material Material;
    public static BookAnimator instance;

    public Animator Animator
    {
        get { return _animator; }
        set { _animator = value; }
    }

    public Image Character
    {
        get { return _character; }
        set { _character = value; }
    }

    public TextMeshProUGUI CharacterName
    {
        get { return _characterName; }
        set { _characterName = value; }
    }

    public List<Sprite> CharacterList
    {
        get { return _characterList; }
        set { _characterList = value; }
    }

    public List<string> NameList
    {
        get { return _nameList; }
        set { _nameList = value; }
    }

    public EndlessBook Book
    {
        get { return _book; }
        set { _book = value; }
    }

    public BookEnum BookEnum
    {
        get { return _bookEnum; }
        set { _bookEnum = value; }
    }


    // Start is called before the first frame update
    void Start()
    {
        if (instance == null&&this.gameObject.name== "PageView_Character")
        {
            instance = this;
            Debug.Log($"name:{this.gameObject.name}");
        }
        else
        {
        }
    }

    private void OnEnable()
    {
        if (Description && Text != null)
        {
            StartTyping();
        }

        if (_animator)
        {
            _animator.SetTrigger("Active");
        }
    }


    public void ChangeImage(int currentPage)
    {
        if (((currentPage) / 2-1) < CharacterList.Count&& ((currentPage) / 2-1)>=0)
        {
            Debug.Log($"index={(currentPage)}");
            Debug.Log($"Listindex={(currentPage) / 2 - 1}");
            Character.sprite = CharacterList[(currentPage)/2-1];
            CharacterName.text = NameList[(currentPage) / 2 - 1];

        }
        else
        {
            Debug.Log($"index={currentPage}");
            Debug.Log($"Listindex={(currentPage) / 2 - 1}");
        }

    }


    public void Active(Material TempMateral)
    {
        if (TempMateral == Material)
        {
            StartTyping();
        }
    }

    public void StartTyping()
    {
        StopAllCoroutines(); // ���֮ǰ���ڴ��֣���ֹͣ
        StartCoroutine(TypeText());
    }

    private IEnumerator TypeText()
    {
        Description.text = "";
        foreach (char letter in Text)
        {
            Description.text += letter;
            yield return new WaitForSeconds(0.05f); // ÿ����֮��ļ��ʱ�䣨�ɵ���
        }
    }
}
