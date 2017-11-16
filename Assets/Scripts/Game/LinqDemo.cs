using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LinqPerson {
	public int age = 0;
	public string name = "Bill";
}

public class LinqDemo : MonoBehaviour
{

	List<LinqPerson> people = new List<LinqPerson> {
		new LinqPerson {
			age=10,
			name="Phil"
		},
		new LinqPerson {
			age=12,
			name="Bill"
		},
		new LinqPerson {
			age = 20,
			name= "Potato"
		},
		new LinqPerson {
			age = 5,
			name = "Bob"
		}
	};
	void Start ()
	{
		List<LinqPerson> selected = people.Where(item => item.age > 7 && item.age < 14).OrderBy(item => item.name).ToList();
		foreach (LinqPerson person in selected) {
			Debug.Log(person.age + " " + person.name);
		}
	}
	
	void Update ()
	{
	}
}
