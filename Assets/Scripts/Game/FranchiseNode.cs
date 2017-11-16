using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Person {
	public int age = 0;
	public string name = "Bill";
}

public class FranchiseNode : MonoBehaviour
{

	List<Person> people = new List<Person> {
		new Person {
			age=10,
			name="Phil"
		},
		new Person {
			age=12,
			name="Bill"
		},
		new Person {
			age = 20,
			name= "Potato"
		},
		new Person {
			age = 5,
			name = "Bob"
		}
	};
	void Start ()
	{
		List<Person> selected = people.Where(item => item.age > 7 && item.age < 14).OrderBy(item => item.name).ToList();
		foreach (Person person in selected) {
			Debug.Log(person.age + " " + person.name);
		}
	}
	
	void Update ()
	{
	}
}
