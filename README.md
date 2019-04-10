# Using Git (2018.3.0b7 and higher)
## Installation

* Open`manifest.json` in PROJECT_NAME/Packages/
* Add `"atuvu.allocation": "https://github.com/Qc-Leblond/AtuvuEngine-Allocation.git"` to the dependencies. It should look similar to this:
```
{
  "dependencies": {
    "atuvu.allocation": "https://github.com/Qc-Leblond/AtuvuEngine-Allocation.git",
    "com.unity.ads": "2.0.8",
    "com.unity.analytics": "3.2.2",
    [...]
  }
}
```

## Update
* Remove the atuvu-allocation part of the lock segment in the manifest.json. The lock part is generated when importing a git package, it will look like this:
```
"lock": {
    "atuvu.allocation": {
      "hash": "04f06ea8a848ecd376142cee6962841b50d3a65f",
      "revision": "HEAD"
    }
  }
```



# Using a Local Version
## Installation
* Pull repository. It is a good idea to have this repository inside your project folder to be able to submit it to version control and link it with a relative path instead of an absolute path.
* Open`manifest.json` in `PROJECT_NAME/Packages/`.
* Add `"atuvu.allocation": "PATH_TO_REPOSITORY/AtuvuEngine-Allocation"` to the dependencies. It should look similar to the following example. In this case, AtuvuEngine-Allocation repository is in the project folder (at the same level as the Assets folder):
```
{
  "dependencies": {
    "atuvu.allocation": "../AtuvuEngine-Allocation",
    "com.unity.ads": "2.0.8",
    "com.unity.analytics": "3.2.2",
    [...]
  }
}
```

## Update
* Pull repository
