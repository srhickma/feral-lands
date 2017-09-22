using UnityEngine;
using System.Collections;

public class Recipe {

	public int[] g;
	
	public Recipe(int[] a){
		if(a.Length == 28)g = a;
	}

	public bool equals(Recipe c){
		for(int i  = 0; i < 25; i ++)
			if(g[i] != c.g[i])return false;
		return true;
	}
}
