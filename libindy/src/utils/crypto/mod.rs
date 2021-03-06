#[cfg(feature = "box_sodium")]
#[path = "box_/sodium.rs"]
pub mod box_;

#[cfg(feature = "base58_rust_base58")]
#[path = "base58/rust_base58.rs"]
pub mod base58;

#[allow(dead_code)] /* FIXME */
#[cfg(feature = "xsalsa20_sodium")]
#[path = "xsalsa20/sodium.rs"]
pub mod xsalsa20;

#[cfg(feature = "chacha20poly1305_ietf_sodium")]
#[path = "chacha20poly1305_ietf/sodium.rs"]
pub mod chacha20poly1305_ietf;

#[cfg(feature = "hash_openssl")]
#[path = "hash/openssl.rs"]
pub mod hash;

pub mod signature_serializer;

pub mod verkey_builder;


#[cfg(feature = "sealedbox_sodium")]
#[path = "sealedbox/sodium.rs"]
pub mod sealedbox;

#[cfg(feature = "pwhash_argon2i13_sodium")]
#[path = "pwhash_argon2i13/sodium.rs"]
pub mod pwhash_argon2i13;