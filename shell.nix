{ pkgs ? import ./nix/pinned-nixpkgs.nix {}
, lib ? pkgs.stdenv.lib
}:

pkgs.stdenv.mkDerivation rec {
  name = "dev-shell";

  buildInputs = with pkgs; [
    fontconfig
    mono6
    xorg.libX11
    xorg.libX11.dev
    zlib
  ];

  LD_LIBRARY_PATH = "${lib.makeLibraryPath buildInputs}";
}
