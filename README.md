# Horatius

[![Build Status](https://travis-ci.org/mtrsk/Horatius.svg?branch=master)](https://travis-ci.org/mtrsk/Horatius)
[![Build Status](https://img.shields.io/endpoint.svg?url=https%3A%2F%2Factions-badge.atrox.dev%2Fmtrsk%2FHoratius%2Fbadge%3Fref%3Dmaster&style=flat)](https://actions-badge.atrox.dev/mtrsk/Horatius/goto?ref=master)

```
     Then out spake brave Horatius,
          The Captain of the Gate:
     "To every man upon this earth
          Death cometh soon or late.
     And how can man die better
          Than facing fearful odds,
     For the ashes of his fathers,
          And the temples of his gods,
```
[Lays of Ancient Rome - Horatius, XXVII.](https://en.wikisource.org/wiki/Lays_of_Ancient_Rome)

## Structure

```
├── Dockerfile
├── files
│   ├── matriculasSemDV.txt
│   └── matriculasParaVerificar.txt
├── Horatius.sln
├── nix
│   ├── .nixpkgs-version.json
│   └── pinned-nixpkgs.nix
├── README.md
├── shell.nix
└── src
    ├── App
    │   ├── App.fsproj
    │   ├── AppView.fs
    │   ├── Lib.fs
    │   └── Main.fs
    ├── CLI
    │   ├── CLI.fsproj
    │   └── Program.fs
    ├── Verification
    │   ├── Library.fs
    │   └── Verification.fsproj
    └── Verification.Tests
        ├── Program.fs
        ├── Tests.fs
        └── Verification.Tests.fsproj
```

### Source Code

* Verification: A classlib which implements the main components of the project.
* Verification.Test: Runs XUnit testes and property based tests (with `FsCheck`).
* CLI: A console application that runs the `Verification` lib on the given files.
* App: The GUI Application.

